

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkRoleSimplelistResult
{
    [JsonProperty("userid")]
    [System.Text.Json.Serialization.JsonPropertyName("userid")]
    public string userid { get; set; }

    [JsonProperty("name")]
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string name { get; set; }
}