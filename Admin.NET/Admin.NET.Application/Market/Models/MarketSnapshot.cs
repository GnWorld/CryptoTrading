// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

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

