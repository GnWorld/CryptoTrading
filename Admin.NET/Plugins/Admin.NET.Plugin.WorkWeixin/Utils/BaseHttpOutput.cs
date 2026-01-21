

namespace Admin.NET.Plugin.WorkWeixin;

/// <summary>
/// 企业微信接口输出基类
/// </summary>
public class BaseWorkOutput
{
    /// <summary>
    /// 返回码
    /// </summary>
    [JsonProperty("errcode")]
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }

    /// <summary>
    /// 对返回码的文本描述内容
    /// </summary>
    [JsonProperty("errmsg")]
    [JsonPropertyName("errmsg")]
    public string ErrMsg { get; set; }
}

/// <summary>
/// 带id的输出参数
/// </summary>
public class BaseWorkIdOutput : BaseWorkOutput
{
    /// <summary>
    /// id
    /// </summary>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public long? Id { get; set; }
}