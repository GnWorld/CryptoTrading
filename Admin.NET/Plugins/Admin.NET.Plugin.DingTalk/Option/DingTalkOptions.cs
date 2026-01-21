

namespace Admin.NET.Plugin.DingTalk;

public sealed class DingTalkOptions : IConfigurableOptions
{
    /// <summary>
    /// AppId
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// AgentId
    /// </summary>
    public string AgentId { get; set; }

    /// <summary>
    /// 原 AppKey 和 SuiteKey
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 原 AppSecret 和 SuiteSecret
    /// </summary>
    public string ClientSecret { get; set; }
}