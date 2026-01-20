namespace Admin.NET.Application.Market.Models;

/// <summary>
/// 单交易对的最新行情快照（内存唯一真相）
/// </summary>
public class MarketSnapshot
{
    /// <summary>交易对，如 BTCUSDT</summary>
    public string Symbol { get; set; } = default!;

    /// <summary>标记价格（强平、风控唯一依据）</summary>
    public decimal MarkPrice { get; set; }

    /// <summary>指数价格（现货参考价）</summary>
    public decimal IndexPrice { get; set; }

    /// <summary>最优买价</summary>
    public decimal BestBid { get; set; }

    /// <summary>最优卖价</summary>
    public decimal BestAsk { get; set; }

    /// <summary>更新时间</summary>
    public DateTime Timestamp { get; set; }
}

