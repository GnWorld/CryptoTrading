

namespace Admin.NET.Core;

public sealed class DeepSeekOptions : IConfigurableOptions
{
    /// <summary>
    /// 源语言
    /// </summary>
    public string SourceLang { get; set; }

    /// <summary>
    /// Api地址
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// API KEY
    /// </summary>
    public string ApiKey { get; set; }
}