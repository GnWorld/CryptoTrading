

namespace Admin.NET.Plugin.DingTalk;

public class DingTalkWorkflowProcessInstancesInput
{
    /// <summary>
    /// 发起人用户ID
    /// </summary>
    public string OriginatorUserId { get; set; }

    /// <summary>
    /// 审批模板的流程编码
    /// </summary>
    public string ProcessCode { get; set; }

    /// <summary>
    /// 部门ID
    /// </summary>
    public long DeptId { get; set; }

    /// <summary>
    /// 微应用AgentId
    /// </summary>
    public long MicroappAgentId { get; set; }

    /// <summary>
    /// 审批人列表（支持多节点）
    /// </summary>
    public List<Approver> Approvers { get; set; }

    /// <summary>
    /// 抄送人列表
    /// </summary>
    public List<string> CcList { get; set; }

    /// <summary>
    /// 抄送位置：START（开始），MIDDLE（中间），END（结束）
    /// </summary>
    public string CcPosition { get; set; }

    /// <summary>
    /// 目标动态选择办理人（用于会签或或签等场景）
    /// </summary>
    public List<TargetSelectActioner> TargetSelectActioners { get; set; }

    /// <summary>
    /// 表单组件值列表
    /// </summary>
    public List<FormComponentValue> FormComponentValues { get; set; }

    /// <summary>
    /// 请求ID，用于幂等控制
    /// </summary>
    public string RequestId { get; set; }
}

/// <summary>
/// 审批人信息
/// </summary>
public class Approver
{
    /// <summary>
    /// 节点类型：AGREE（同意），REFUSE（拒绝）等
    /// </summary>
    public string ActionType { get; set; }

    /// <summary>
    /// 该节点的审批人用户ID列表
    /// </summary>
    public List<string> UserIds { get; set; }
}

/// <summary>
/// 动态选择办理人
/// </summary>
public class TargetSelectActioner
{
    /// <summary>
    /// 办理人Key，对应表单中的人员选择控件的key
    /// </summary>
    public string ActionerKey { get; set; }

    /// <summary>
    /// 该控件选中的用户ID列表
    /// </summary>
    public List<string> ActionerUserIds { get; set; }
}

/// <summary>
/// 表单组件值
/// </summary>
public class FormComponentValue
{
    public string ComponentType { get; set; }
    public string Name { get; set; }
    public string BizAlias { get; set; }
    public string Id { get; set; }
    public string Value { get; set; }
    public string ExtValue { get; set; }
}