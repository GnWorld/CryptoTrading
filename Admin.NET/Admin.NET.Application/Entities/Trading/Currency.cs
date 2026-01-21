

using Admin.NET.Application.Enums;
using SqlSugar;

namespace Admin.NET.Application.Entity.Trading;

[SugarTable(null,"币种表")]
public class Currency : EntityBaseTenantOrgDel
{
    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "币种编码")]
    public string Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "币种名称")]
    public string Name { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(ColumnDescription = "币种图标")]
    public string Icon { get; set; }

    /// <summary>
    /// 币种类型  0-法币 1-虚拟货币
    /// </summary>
    [SugarColumn(ColumnDescription = "币种类型 0-法币 1-虚拟货币")]
    public CurType CurType { get; set; }

    /// <summary>
    /// 锚定币种代码
    /// </summary>
    [SugarColumn(ColumnDescription = "锚定币种代码 默认锚定USD")]
    public string AnchorCurCode { get; set; }

    /// <summary>
    /// 锚定汇率
    /// </summary>
    [SugarColumn(ColumnDescription = "锚定币种汇率 统一使用 USD/XXX ")]
    public decimal AnchorRate { get; set; }

    /// <summary>
    /// 是否刷新锚定汇率  0-否 1-是
    /// </summary>
    [SugarColumn(ColumnDescription = "是否刷新汇率")]
    public bool IsFlushAnchorRate { get; set; }
}
