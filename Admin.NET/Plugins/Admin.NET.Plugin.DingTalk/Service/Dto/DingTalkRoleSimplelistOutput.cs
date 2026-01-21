

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkRoleSimplelistOutput
{
    /// <summary>
    /// 是否还有更多数据
    /// </summary>
    [JsonProperty("hasMore")]
    [System.Text.Json.Serialization.JsonPropertyName("hasMore")]
    public bool hasMore { get; set; }

    /// <summary>
    /// 角色组列表
    /// </summary>
    [JsonProperty("list")]
    [System.Text.Json.Serialization.JsonPropertyName("list")]
    public List<DingTalkRoleSimplelistResult> list { get; set; }
}