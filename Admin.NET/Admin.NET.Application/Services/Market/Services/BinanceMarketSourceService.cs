

using Admin.NET.Application.Entities.Trading;
using Admin.NET.Application.Market.Models;
using Binance.Net.Clients;
using Microsoft.Extensions.Hosting;

namespace Admin.NET.Application.Market.Services;


/// <summary>
/// 币安行情源
/// </summary>
public class BinanceMarketSourceService : IHostedService
{


    //private readonly MarketSnapshotService marketCache;
    private readonly IServiceScopeFactory _scopeFactory;
    public BinanceMarketSourceService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
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
            $"Ask: {s.AskPrice:F2} ({s.AskQty})"
        );
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var marketCache = scope.ServiceProvider.GetRequiredService<MarketSnapshotService>();
        var instrumentRepo =
           scope.ServiceProvider.GetRequiredService<
               SqlSugarRepository<TradeInstrument>>();
        var insList = await instrumentRepo.GetListAsync(x => x.MarketSource.Contains("Binance"));
        var symbols = insList.Select(x => x.BaseCurCode + x.QuoteCurCode).ToList();
        if (symbols.Count == 0)
        {
            symbols.Add("BTCUSDT");
            symbols.Add("ETHUSDT");
        }
        var client = new BinanceSocketClient();

        //订阅标记价
        await client.UsdFuturesApi.ExchangeData.SubscribeToMarkPriceUpdatesAsync(
                symbols,
                3000,
                data =>
                {
                    var snapshot = marketCache.GetOrAdd(
                        data.Data.Symbol);
                    if (snapshot.UpdateMark(
                           data.Data.MarkPrice,
                           data.Data.IndexPrice))
                    {
                        Print(snapshot);
                    }
                }, cancellationToken);

        //订阅买卖价
        await client.UsdFuturesApi.ExchangeData.SubscribeToBookTickerUpdatesAsync(symbols, data =>
        {
            var snapshot = marketCache.GetOrAdd(
                data.Data.Symbol);
            if (snapshot.UpdateBook(
                data.Data.BestBidPrice,
                data.Data.BestBidQuantity,
                data.Data.BestAskPrice,
                data.Data.BestAskQuantity))
            {
                Print(snapshot);
            }
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
