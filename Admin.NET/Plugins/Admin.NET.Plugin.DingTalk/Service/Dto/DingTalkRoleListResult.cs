

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkRoleListResult
{
    [JsonProperty("groupId")]
    [System.Text.Json.Serialization.JsonPropertyName("groupId")]
    public long groupId { get; set; }

    [JsonProperty("name")]
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string name { get; set; }

    [JsonProperty("roles")]
    [System.Text.Json.Serialization.JsonPropertyName("roles")]
    public List<DingTalkRoleResult> roles { get; set; }
}