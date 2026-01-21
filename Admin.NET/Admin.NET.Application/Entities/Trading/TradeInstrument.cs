using SqlSugar;

namespace Admin.NET.Application.Entities.Trading;

[SugarTable(null, "交易品种")]
public class TradeInstrument : EntityBaseTenantOrgDel
{
    /// <summary>
    /// 交易对标识（如：BTCUSDT）
    /// </summary>
    [SugarColumn(ColumnDescription = "交易对标识")]
    public string Symbol { get; set; }

    /// <summary>
    /// 基础币种编码
    /// </summary>
    [SugarColumn(ColumnDescription = "基础币种编码")]
    public string BaseCurCode { get; set; }

    /// <summary>
    /// 计价币种编码   比如BTCUSDT  BTC是基础币种 USDT是计价币种，平常也会说BTC价值多少多少美元了(USDT 和USD 基本价格稳定一致)
    /// </summary>
    [SugarColumn(ColumnDescription = "计价币种编码")]
    public string QuoteCurCode { get; set; }

    /// <summary>
    /// 展示名称（如：BTC/USDT 永续）
    /// </summary>
    [SugarColumn(ColumnDescription = "展示名称")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 保证金币种（结算币种）
    /// </summary>
    [SugarColumn(ColumnDescription = "保证金币种编码")]
    public string MarginCurCode { get; set; }

    /// <summary>
    /// 是否永续合约 0-交割合约 1-永续合约
    /// </summary>
    [SugarColumn(ColumnDescription = "是否永续合约")]
    public bool IsPerpetual { get; set; }

    /// <summary>
    /// 最大杠杆倍数
    /// </summary>
    [SugarColumn(ColumnDescription = "最大杠杆倍数")]
    public int MaxLeverage { get; set; }

    /// <summary>
    /// 价格精度（小数位）
    /// </summary>
    [SugarColumn(ColumnDescription = "价格精度")]
    public int PricePrecision { get; set; }

    /// <summary>
    /// 数量精度（小数位）
    /// </summary>
    [SugarColumn(ColumnDescription = "数量精度")]
    public int QuantityPrecision { get; set; }

    /// <summary>
    /// 最小下单数量
    /// </summary>
    [SugarColumn(ColumnDescription = "最小下单数量")]
    public decimal MinOrderQty { get; set; }

    /// <summary>
    /// 最大下单数量
    /// </summary>
    [SugarColumn(ColumnDescription = "最大下单数量")]
    public decimal MaxOrderQty { get; set; }

    /// <summary>
    /// 最小名义金额（计价币种）
    /// </summary>
    [SugarColumn(ColumnDescription = "最小名义金额")]
    public decimal MinNotional { get; set; }

    /// <summary>
    /// 最小价格步长
    /// </summary>
    [SugarColumn(ColumnDescription = "价格步长")]
    public decimal TickSize { get; set; }

    /// <summary>
    /// 最小数量步长
    /// </summary>
    [SugarColumn(ColumnDescription = "数量步长")]
    public decimal StepSize { get; set; }

    /// <summary>
    /// Maker 手续费率
    /// </summary>
    [SugarColumn(ColumnDescription = "Maker 手续费率")]
    public decimal MakerFeeRate { get; set; }

    /// <summary>
    /// Taker 手续费率
    /// </summary>
    [SugarColumn(ColumnDescription = "Taker 手续费率")]
    public decimal TakerFeeRate { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [SugarColumn(ColumnDescription = "是否启用")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnDescription = "排序号")]
    public int Sort { get; set; }

    /// <summary>
    /// 市场来源  比如Binance  OKX  etc
    /// </summary>
    [SugarColumn(ColumnDescription = "市场来源")]
    public string MarketSource { get; set; }
}
