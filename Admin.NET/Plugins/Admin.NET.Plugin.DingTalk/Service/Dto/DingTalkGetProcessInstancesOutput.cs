

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkGetProcessInstancesOutput
{
    public ResultData Result { get; set; }
    public bool Success { get; set; }
}

public class OperationRecord
{
    public DateTime? Date { get; set; }
    public string Result { get; set; }
    public List<object> Images { get; set; } // 图片可能是字符串 URL 或对象
    public string ShowName { get; set; }
    public string Type { get; set; }
    public string UserId { get; set; }
}

// 表格行中的子项（用于 TableField 的解析）
public class TableRowItem
{
    public string BizAlias { get; set; }
    public string Label { get; set; }
    public string Value { get; set; }
    public string Key { get; set; }
    public bool Mask { get; set; }
}

// 完整的一行表格数据
public class TableRow
{
    public List<TableRowItem> RowValue { get; set; }
    public string RowNumber { get; set; }
}

public class TaskItem
{
    public string Result { get; set; }
    public string ActivityId { get; set; }
    public string PcUrl { get; set; }
    public DateTime? CreateTime { get; set; }
    public string MobileUrl { get; set; }
    public string UserId { get; set; }
    public long TaskId { get; set; }
    public string Status { get; set; }
}

public class ResultData
{
    public List<string> AttachedProcessInstanceIds { get; set; }
    public string BusinessId { get; set; }
    public string Title { get; set; }
    public string OriginatorDeptId { get; set; }
    public List<OperationRecord> OperationRecords { get; set; }
    public List<FormComponentValue> FormComponentValues { get; set; }

    /// <summary>
    /// 审批结果 agree：同意 refuse：拒绝
    /// </summary>
    public string Result { get; set; }

    public string BizAction { get; set; }
    public DateTime? CreateTime { get; set; }
    public string OriginatorUserId { get; set; }
    public List<TaskItem> Tasks { get; set; }
    public string OriginatorDeptName { get; set; }
    public string Status { get; set; }
}