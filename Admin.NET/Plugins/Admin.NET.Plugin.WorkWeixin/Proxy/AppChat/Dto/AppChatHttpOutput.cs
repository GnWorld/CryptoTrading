

namespace Admin.NET.Plugin.WorkWeixin.Proxy;

public class CreatAppChatOutput : BaseWorkOutput
{
    /// <summary>
    /// 群聊的唯一标志
    /// </summary>
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public string ChatId { get; set; }
}