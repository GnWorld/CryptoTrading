

namespace Admin.NET.Plugin.WorkWeixin.Proxy;

/// <summary>
/// 标签输入参数
/// </summary>
public class TagHttpInput
{
    /// <summary>
    /// 标签id
    /// </summary>
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public long? TagId { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    [JsonProperty("tagname")]
    [JsonPropertyName("tagname")]
    public string TagName { get; set; }
}

/// <summary>
/// 增加标签成员输入参数
/// </summary>
public class TagUsersTagInput
{
    /// <summary>
    /// 标签id
    /// </summary>
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public long TagId { get; set; }

    /// <summary>
    /// 企业成员ID列表
    /// </summary>
    [JsonProperty("userlist")]
    [JsonPropertyName("userlist")]
    public List<string> UserList { get; set; }

    /// <summary>
    /// 企业部门ID列表
    /// </summary>
    [JsonProperty("partylist")]
    [JsonPropertyName("partylist")]
    public List<long> PartyList { get; set; }
}