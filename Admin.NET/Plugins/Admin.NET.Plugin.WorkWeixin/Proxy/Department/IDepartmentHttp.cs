

namespace Admin.NET.Plugin.WorkWeixin.Proxy.AppChat;

/// <summary>
/// 部门远程调用服务
/// </summary>
public interface IDepartmentHttp : IHttpDeclarative
{
    /// <summary>
    /// 创建部门
    /// https://developer.work.weixin.qq.com/document/path/90205
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [Post("https://qyapi.weixin.qq.com/cgi-bin/department/create")]
    Task<BaseWorkIdOutput> Create([Query("access_token")] string accessToken, [Body] DepartmentHttpInput body);

    /// <summary>
    /// 修改部门
    /// https://developer.work.weixin.qq.com/document/path/90206
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [Post("https://qyapi.weixin.qq.com/cgi-bin/department/update")]
    Task<BaseWorkOutput> Update([Query("access_token")] string accessToken, [Body] DepartmentHttpInput body);

    /// <summary>
    /// 删除部门
    /// https://developer.work.weixin.qq.com/document/path/90207
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Get("https://qyapi.weixin.qq.com/cgi-bin/department/delete")]
    Task<BaseWorkOutput> Delete([Query("access_token")] string accessToken, [Query] long id);

    /// <summary>
    /// 获取部门Id列表
    /// https://developer.work.weixin.qq.com/document/path/90208
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Get("https://qyapi.weixin.qq.com/cgi-bin/department/simplelist")]
    Task<DepartmentIdOutput> SimpleList([Query("access_token")] string accessToken, [Query] long id);

    /// <summary>
    /// 获取部门详情
    /// https://developer.work.weixin.qq.com/document/path/90208
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Get("https://qyapi.weixin.qq.com/cgi-bin/department/get")]
    Task<DepartmentOutput> Get([Query("access_token")] string accessToken, [Query] long id);
}