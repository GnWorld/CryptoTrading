

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkWorkflowProcessInstancesOutput
{
    /// <summary>
    /// 请求Id
    /// </summary>
    [Newtonsoft.Json.JsonProperty("request_id")]
    [System.Text.Json.Serialization.JsonPropertyName("request_id")]
    public string RequestId { get; set; }

    public string code { get; set; }
    public string message { get; set; }

    /// <summary>
    /// 是否还有更多数据
    /// </summary>
    [JsonProperty("instanceId")]
    [System.Text.Json.Serialization.JsonPropertyName("instanceId")]
    public string instanceId { get; set; }
}