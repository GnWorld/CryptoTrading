namespace Admin.NET.Application.Market.Models;

/// <summary>
/// 单交易对的最新行情快照（内存唯一真相）
/// </summary>
public class FuturesMarketSnapshot
{
    public string Symbol { get; set; } = default!;
    public decimal MarkPrice { get; set; }
    public decimal IndexPrice { get; set; }
    public decimal BidPrice { get; set; }
    public decimal BidQty { get; set; }
    public decimal AskPrice { get; set; }
    public decimal AskQty { get; set; }
    public DateTime UpdateTime { get; set; }


    public bool UpdateMark(decimal mark, decimal index)
    {
        if (MarkPrice == mark && IndexPrice == index)
            return false;

        MarkPrice = mark;
        IndexPrice = index;
        UpdateTime = DateTime.UtcNow;
        return true;
    }

    public bool UpdateBook(
        decimal bidPrice, decimal bidQty,
        decimal askPrice, decimal askQty)
    {
        if (BidPrice == bidPrice && BidQty == bidQty
            && AskPrice == askPrice && AskQty == askQty)
            return false;

        BidPrice = bidPrice;
        BidQty = bidQty;
        AskPrice = askPrice;
        AskQty = askQty;
        UpdateTime = DateTime.UtcNow;
        return true;
    }
}
