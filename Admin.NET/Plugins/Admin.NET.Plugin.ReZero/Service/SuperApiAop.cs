

using Admin.NET.Core;
using Furion.DataEncryption;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReZero.SuperAPI;

namespace Admin.NET.Plugin.ReZero.Service;

/// <summary>
/// 超级API接口拦截器
/// </summary>
public class SuperApiAop : DefaultSuperApiAop
{
    public override async Task OnExecutingAsync(InterfaceContext aopContext)
    {
        //if (aopContext.InterfaceType == InterfaceType.DynamicApi)
        //{
        var authenticateResult = await aopContext.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
            throw Oops.Oh("没权限 Unauthorized");
        //}

        var accessToken = aopContext.HttpContext.Request.Headers["Authorization"].ToString();
        var (isValid, tokenData, validationResult) = JWTEncryption.Validate(accessToken.Replace("Bearer ", ""));
        if (!isValid)
            throw Oops.Oh("Token 无效");

        await base.OnExecutingAsync(aopContext);
    }

    public override async Task OnExecutedAsync(InterfaceContext aopContext)
    {
        InitLogContext(aopContext, LogLevel.Information);

        await base.OnExecutedAsync(aopContext);
    }

    public override async Task OnErrorAsync(InterfaceContext aopContext)
    {
        InitLogContext(aopContext, LogLevel.Error);

        await base.OnErrorAsync(aopContext);
    }

    /// <summary>
    /// 保存超级API接口日志
    /// </summary>
    /// <param name="aopContext"></param>
    /// <param name="logLevel"></param>
    private void InitLogContext(InterfaceContext aopContext, LogLevel logLevel)
    {
        var api = aopContext.InterfaceInfo;
        var context = aopContext.HttpContext;

        var accessToken = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrWhiteSpace(accessToken) && accessToken.StartsWith("Bearer "))
            accessToken = accessToken.Replace("Bearer ", "");
        var claims = JWTEncryption.ReadJwtToken(accessToken)?.Claims;
        var userName = claims?.FirstOrDefault(u => u.Type == ClaimConst.Account)?.Value;
        var realName = claims?.FirstOrDefault(u => u.Type == ClaimConst.RealName)?.Value;

        var paths = api.Url.Split('/');
        var actionName = paths[paths.Length - 1];

        var apiInfo = new
        {
            requestUrl = api.Url,
            httpMethod = api.HttpMethod,
            displayTitle = api.Name,
            actionTypeName = actionName,
            controllerName = aopContext.InterfaceType == InterfaceType.DynamicApi ? $"ReZero动态-{api.GroupName}" : $"ReZero系统-{api.GroupName}",
            remoteIPv4 = context.GetRemoteIpAddressToIPv4(),
            userAgent = context.Request.Headers["User-Agent"],
            returnInformation = new
            {
                httpStatusCode = context.Response.StatusCode,
            },
            authorizationClaims = new[]
            {
                new
                {
                    type = ClaimConst.Account,
                    value = userName
                },
                new
                {
                    type = ClaimConst.RealName,
                    value = realName
                },
            },
            exception = aopContext.Exception == null ? null : JSON.Serialize(aopContext.Exception)
        };

        var logger = App.GetRequiredService<ILoggerFactory>().CreateLogger(CommonConst.SysLogCategoryName);
        using var scope = logger.ScopeContext(new Dictionary<object, object> {
            { "loggingMonitor", apiInfo.ToJson() }
        });
        logger.Log(logLevel, "ReZero超级API接口日志");
    }
}