

namespace Admin.NET.Plugin.DingTalk;

/// <summary>
/// 钉钉基础响应结果
/// </summary>
/// <typeparam name="T">Data</typeparam>
public class DingTalkBaseResponse<T>
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public T Result { get; set; }

    /// <summary>
    /// 返回码
    /// </summary>
    public int ErrCode { get; set; }

    /// <summary>
    /// 返回码描述。
    /// </summary>
    public string ErrMsg { get; set; }

    /// <summary>
    /// 是否调用成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 请求Id
    /// </summary>
    [Newtonsoft.Json.JsonProperty("request_id")]
    [System.Text.Json.Serialization.JsonPropertyName("request_id")]
    public string RequestId { get; set; }
}