using SqlSugar;
using System.Data;

namespace Admin.NET.Application.Entities.Trading;

/// <summary>
/// K线数据实体
/// </summary>
[SugarTable(null, "K线数据")]
public class Candlestick : EntityBaseTenantOrgDel
{
    /// <summary>
    /// 交易对标识（如：BTCUSDT）
    /// </summary>
    [SugarColumn(ColumnDescription = "交易对标识", IndexGroupNameList = new string[] { "idx_symbol_interval", "idx_symbol_opentime" })]
    public string Symbol { get; set; }

    /// <summary>
    /// K线周期
    /// </summary>
    [SugarColumn(ColumnDescription = "K线周期", IndexGroupNameList = new string[] { "idx_symbol_interval" })]
    public string Interval { get; set; }

    /// <summary>
    /// 开盘时间
    /// </summary>
    [SugarColumn(ColumnDescription = "开盘时间", IndexGroupNameList = new string[] { "idx_symbol_opentime" })]
    public long OpenTime { get; set; }

    /// <summary>
    /// 开盘价
    /// </summary>
    [SugarColumn(ColumnDescription = "开盘价", ColumnDataType = "decimal(38,18)")]
    public decimal Open { get; set; }

    /// <summary>
    /// 最高价
    /// </summary>
    [SugarColumn(ColumnDescription = "最高价", ColumnDataType = "decimal(38,18)")]
    public decimal High { get; set; }

    /// <summary>
    /// 最低价
    /// </summary>
    [SugarColumn(ColumnDescription = "最低价", ColumnDataType = "decimal(38,18)")]
    public decimal Low { get; set; }

    /// <summary>
    /// 收盘价
    /// </summary>
    [SugarColumn(ColumnDescription = "收盘价", ColumnDataType = "decimal(38,18)")]
    public decimal Close { get; set; }

    /// <summary>
    /// 成交量
    /// </summary>
    [SugarColumn(ColumnDescription = "成交量", ColumnDataType = "decimal(38,18)")]
    public decimal Volume { get; set; }

    /// <summary>
    /// 收盘时间
    /// </summary>
    [SugarColumn(ColumnDescription = "收盘时间")]
    public long CloseTime { get; set; }

    /// <summary>
    /// 计价资产成交量
    /// </summary>
    [SugarColumn(ColumnDescription = "计价资产成交量", ColumnDataType = "decimal(38,18)")]
    public decimal QuoteAssetVolume { get; set; }

    /// <summary>
    /// 成交笔数
    /// </summary>
    [SugarColumn(ColumnDescription = "成交笔数")]
    public int NumberOfTrades { get; set; }

    /// <summary>
    /// 主动买入基础资产成交量
    /// </summary>
    [SugarColumn(ColumnDescription = "主动买入基础资产成交量", ColumnDataType = "decimal(38,18)")]
    public decimal TakerBuyBaseAssetVolume { get; set; }

    /// <summary>
    /// 主动买入计价资产成交量
    /// </summary>
    [SugarColumn(ColumnDescription = "主动买入计价资产成交量", ColumnDataType = "decimal(38,18)")]
    public decimal TakerBuyQuoteAssetVolume { get; set; }
}