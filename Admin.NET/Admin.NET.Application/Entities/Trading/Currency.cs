// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

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
