

namespace Admin.NET.Plugin.WorkWeixin.Proxy;

/// <summary>
/// 创建部门输入参数
/// </summary>
public class DepartmentHttpInput
{
    /// <summary>
    /// 部门名称
    /// </summary>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    /// <summary>
    /// 父部门id
    /// </summary>
    [JsonProperty("parentid")]
    [JsonPropertyName("parentid")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 英文名称
    /// </summary>
    [JsonProperty("name_en")]
    [JsonPropertyName("name_en")]
    public string NameEn { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }
}