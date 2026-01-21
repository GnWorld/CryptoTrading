

namespace Admin.NET.Core;

/// <summary>
/// 微信相关配置选项
/// </summary>
public sealed class WechatOptions : IConfigurableOptions
{
    // 公众号
    public string WechatAppId { get; set; }

    public string WechatAppSecret { get; set; }

    /// <summary>
    /// 微信公众号服务器配置中的令牌(Token)
    /// </summary>
    public string WechatToken { get; set; }

    /// <summary>
    /// 微信公众号服务器配置中的消息加解密密钥(EncodingAESKey)
    /// </summary>
    public string WechatEncodingAESKey { get; set; }

    // 小程序
    public string WxOpenAppId { get; set; }

    public string WxOpenAppSecret { get; set; }

    /// <summary>
    /// 小程序消息推送中的令牌(Token)
    /// </summary>
    public string WxToken { get; set; }

    /// <summary>
    /// 小程序消息推送中的消息加解密密钥(EncodingAESKey)
    /// </summary>
    public string WxEncodingAESKey { get; set; }
}