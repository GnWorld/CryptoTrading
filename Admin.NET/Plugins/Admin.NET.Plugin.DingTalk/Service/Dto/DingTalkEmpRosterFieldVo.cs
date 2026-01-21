

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkEmpRosterFieldVo
{
    /// <summary>
    /// 企业的corpid
    /// </summary>
    [Newtonsoft.Json.JsonProperty("corp_id")]
    [System.Text.Json.Serialization.JsonPropertyName("corp_id")]
    public string CorpId { get; set; }

    /// <summary>
    /// 返回的字段信息列表
    /// </summary>
    [Newtonsoft.Json.JsonProperty("field_data_list")]
    [System.Text.Json.Serialization.JsonPropertyName("field_data_list")]
    public List<DingTalkEmpFieldDataVo> FieldDataList { get; set; }

    /// <summary>
    /// 员工的userid
    /// </summary>
    [Newtonsoft.Json.JsonProperty("userid")]
    [System.Text.Json.Serialization.JsonPropertyName("userid")]
    public string UserId { get; set; }
}