

using System.Text.Json.Serialization;

namespace Admin.NET.Plugin.ApprovalFlow.Service;

public class ApprovalFormItem
{
    [JsonPropertyName("configId")]
    public string ConfigId { get; set; }

    [JsonPropertyName("tableName")]
    public string TableName { get; set; }

    [JsonPropertyName("entityName")]
    public string EntityName { get; set; }

    [JsonPropertyName("typeName")]
    public string TypeName { get; set; }

    [JsonPropertyName("route")]
    public string Route => EntityName[..1].ToLower() + EntityName[1..] + "/" + TypeName;
}