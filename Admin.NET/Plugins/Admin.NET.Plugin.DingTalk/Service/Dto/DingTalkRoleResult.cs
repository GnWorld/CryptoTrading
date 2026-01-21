

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkRoleResult
{
    [JsonProperty("id")]
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public long id { get; set; }

    [JsonProperty("name")]
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string name { get; set; }
}