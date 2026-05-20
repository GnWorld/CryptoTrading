using Admin.NET.Application.Entities.Trading;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using SqlSugar;
using System.Collections.Concurrent;

namespace Admin.NET.Application.Market.Services;

/// <summary>
/// K线数据服务
/// </summary>
public class CandlestickService : IScoped
{


    // 存储每个交易对每个周期的当前K线周期结束时间
    private readonly ConcurrentDictionary<string, long> _currentPeriodEndTimes
    = new();
    // 用于同步同一交易对和周期的K线数据处理
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SqlSugarRepository<Candlestick> _candlestickRepo;

    public CandlestickService(IServiceScopeFactory scopeFactory, SqlSugarRepository<Candlestick> candlestickRepo)
    {

        _scopeFactory = scopeFactory;
        _candlestickRepo = candlestickRepo;
    }

    /// <summary>
    /// 处理K线数据更新
    /// </summary>
    /// <param name="symbol">交易对</param>
    /// <param name="interval">K线周期</param>
    /// <param name="klineData">K线数据</param>
    /// <returns></returns>
    public async Task HandleKlineUpdateAsync(string symbol, string interval, IBinanceStreamKlineData klineData)
    {
        try
        {
            bool isFinal = false;
            var binanceKline = new BinanceStreamKline
            {
                OpenTime = klineData.Data.OpenTime,
                CloseTime = klineData.Data.CloseTime,
                OpenPrice = klineData.Data.OpenPrice,
                HighPrice = klineData.Data.HighPrice,
                LowPrice = klineData.Data.LowPrice,
                ClosePrice = klineData.Data.ClosePrice,
                Volume = klineData.Data.Volume,
                QuoteVolume = klineData.Data.QuoteVolume,
                TradeCount = klineData.Data.TradeCount,
                Final = klineData.Data.Final,
                Symbol = klineData.Symbol,
                Interval = klineData.Data.Interval
            };
            isFinal = binanceKline.Final;

            // 转换为数据库模型所需的字段
            long openTime = new DateTimeOffset(binanceKline.OpenTime).ToUnixTimeMilliseconds();
            decimal open = binanceKline.OpenPrice;
            decimal high = binanceKline.HighPrice;
            decimal low = binanceKline.LowPrice;
            decimal close = binanceKline.ClosePrice;
            decimal volume = binanceKline.Volume;
            long closeTime = new DateTimeOffset(binanceKline.CloseTime).ToUnixTimeMilliseconds();

            // 验证关键数据
            if (openTime == 0 || close == 0)
            {
                Console.WriteLine($"K线数据解析失败，关键字段为0: {symbol} {interval}");
                return;
            }

            // 获取当前时间戳（毫秒）
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 计算K线周期的毫秒数
            long periodMs = GetPeriodMilliseconds(interval);

            // 计算当前K线的周期结束时间
            long periodEndTime = openTime + periodMs;

            // 生成字典键（交易对+周期）
            string key = $"{symbol}_{interval}";

            // 检查是否跨周期或当前周期结束
            if (!_currentPeriodEndTimes.TryGetValue(key, out long lastPeriodEndTime)
               || periodEndTime > lastPeriodEndTime
               || currentTime >= periodEndTime
               || isFinal)
            {
                // 跨周期或当前周期结束，存储K线数据
                var candlestick = new Candlestick
                {
                    Symbol = symbol,
                    Interval = interval,
                    OpenTime = openTime,
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = volume,
                    CloseTime = closeTime,
                    QuoteAssetVolume = binanceKline.QuoteVolume,
                    NumberOfTrades = binanceKline.TradeCount,
                    TakerBuyBaseAssetVolume = binanceKline.TakerBuyBaseVolume,
                    TakerBuyQuoteAssetVolume = binanceKline.TakerBuyQuoteVolume
                };

                Console.WriteLine("原始数据: " + System.Text.Json.JsonSerializer.Serialize(klineData));

                // 查询是否存在
                var existing = new Candlestick();


                // 更新现有数据
                existing.Open = open;
                existing.High = high;
                existing.Low = low;
                existing.Close = close;
                existing.Volume = volume;
                existing.CloseTime = closeTime;
                existing.QuoteAssetVolume = binanceKline.QuoteVolume;
                existing.NumberOfTrades = binanceKline.TradeCount;
                existing.TakerBuyBaseAssetVolume = binanceKline.TakerBuyBaseVolume;
                existing.TakerBuyQuoteAssetVolume = binanceKline.TakerBuyQuoteVolume;
                await _candlestickRepo.InsertOrUpdateAsync(existing);

                // 更新当前周期结束时间（线程安全的方式）

                _currentPeriodEndTimes[key] = periodEndTime;


                Console.WriteLine($"存储K线数据: {symbol} {interval} {openTime} {close}");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"处理K线数据异常: {symbol} {interval}, 错误: {ex.Message}");
            Console.WriteLine($"异常堆栈: {ex.StackTrace}");
        }
    }

    /// <summary>
    /// 获取K线周期的毫秒数
    /// </summary>
    /// <param name="interval">K线周期</param>
    /// <returns>毫秒数</returns>
    private long GetPeriodMilliseconds(string interval)
    {
        switch (interval.ToLower())
        {
            case "1m":
                return 60L * 1000L;
            case "5m":
                return 5L * 60L * 1000L;
            case "15m":
                return 15L * 60L * 1000L;
            case "30m":
                return 30L * 60L * 1000L;
            case "1h":
                return 60L * 60L * 1000L;
            case "2h":
                return 2L * 60L * 60L * 1000L;
            case "4h":
                return 4L * 60L * 60L * 1000L;
            case "6h":
                return 6L * 60L * 60L * 1000L;
            case "12h":
                return 12L * 60L * 60L * 1000L;
            case "1d":
                return 24L * 60L * 60L * 1000L;
            case "3d":
                return 3L * 24L * 60L * 60L * 1000L;
            case "1w":
                return 7L * 24L * 60L * 60L * 1000L;
            case "1mth":
                return 30L * 24L * 60L * 60L * 1000L;
            default:
                return 60L * 1000L; // 默认1分钟
        }
    }

    ///// <summary>
    ///// 获取K线数据
    ///// </summary>
    ///// <param name="symbol">交易对标识</param>
    ///// <param name="interval">K线周期</param>
    ///// <param name="limit">返回数量</param>
    ///// <param name="startTime">开始时间戳（毫秒）</param>
    ///// <param name="endTime">结束时间戳（毫秒）</param>
    ///// <returns>K线数据列表</returns>
    //public async Task<List<Candlestick>> GetCandlesticksAsync(
    //    string symbol,
    //    string interval,
    //    int limit = 100,
    //    long? startTime = null,
    //    long? endTime = null)
    //{
    //    var query = _candlestickRepo.AsQueryable()
    //        .Where(it => it.Symbol == symbol && it.Interval == interval);

    //    // 添加时间范围过滤
    //    if (startTime.HasValue)
    //    {
    //        query = query.Where(it => it.OpenTime >= startTime.Value);
    //    }

    //    if (endTime.HasValue)
    //    {
    //        query = query.Where(it => it.OpenTime <= endTime.Value);
    //    }

    //    // 按开盘时间倒序，然后取最新的N条
    //    var candlesticks = await query
    //        .OrderByDescending(it => it.OpenTime)
    //        .Take(limit)
    //        .ToListAsync();

    //    // 按开盘时间正序返回，符合前端展示习惯
    //    return candlesticks.OrderBy(it => it.OpenTime).ToList();
    //}
}