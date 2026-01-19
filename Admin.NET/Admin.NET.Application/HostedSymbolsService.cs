// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Binance.Net.Clients;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application;

public class HostedSymbolsService : IHostedService
{


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var marketCache = new ConcurrentDictionary<string, FuturesMarketSnapshot>();
        var symbols = new[] { "BTCUSDT" };

        var client = new BinanceSocketClient();

        await client.UsdFuturesApi.ExchangeData.SubscribeToMarkPriceUpdatesAsync(
                symbols,
                3000,
                data =>
                {
                    var snapshot = marketCache.GetOrAdd(
                        data.Data.Symbol,
                        s => new FuturesMarketSnapshot { Symbol = s });

                    snapshot.MarkPrice = data.Data.MarkPrice;
                    snapshot.IndexPrice = data.Data.IndexPrice;
                    snapshot.UpdateTime = DateTime.UtcNow;

                    Print(snapshot);
                });
        await client.UsdFuturesApi.ExchangeData.SubscribeToBookTickerUpdatesAsync(symbols, data =>
            {
                var snapshot = marketCache.GetOrAdd(
                    data.Data.Symbol,
                    s => new FuturesMarketSnapshot { Symbol = s });

                snapshot.BidPrice = data.Data.BestBidPrice;
                snapshot.BidQty = data.Data.BestBidQuantity;
                snapshot.AskPrice = data.Data.BestAskPrice;
                snapshot.AskQty = data.Data.BestAskQuantity;
                snapshot.UpdateTime = DateTime.UtcNow;
                Print(snapshot);
            });

    }
    static void Print(FuturesMarketSnapshot s)
    {
        if (s.MarkPrice == 0 || s.BidPrice == 0 || s.AskPrice == 0)
            return;

        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] {s.Symbol} | " +
            $"Mark: {s.MarkPrice:F2} | " +
            $"Index: {s.IndexPrice:F2} | " +
            $"Bid: {s.BidPrice:F2} ({s.BidQty}) | " +
            $"Ask: {s.AskPrice} ({s.AskQty})"
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
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
}
