using Admin.NET.Application.Entities.Trading;
using Admin.NET.Application.Market.Services;
using Microsoft.AspNetCore.Mvc;

namespace Admin.NET.Web.Entry.Controllers;

/// <summary>
/// 行情控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MarketController : ControllerBase
{
    private readonly MarketSnapshotService _marketSnapshotService;
    private readonly CandlestickService _candlestickService;

    public MarketController(
        MarketSnapshotService marketSnapshotService,
        CandlestickService candlestickService)
    {
        _marketSnapshotService = marketSnapshotService;
        _candlestickService = candlestickService;
    }

    /// <summary>
    /// 获取单个交易对最新行情
    /// </summary>
    /// <param name="symbol">交易对标识</param>
    /// <returns></returns>
    [HttpGet("ticker/{symbol}")]
    public IActionResult GetTicker(string symbol)
    {
        var ticker = _marketSnapshotService.GetOrAdd(symbol);
        return Ok(ticker);
    }

    /// <summary>
    /// 获取所有交易对最新行情
    /// </summary>
    /// <returns></returns>
    [HttpGet("tickers")]
    public IActionResult GetTickers()
    {
        var tickers = _marketSnapshotService.GetAllSnapshots();
        return Ok(tickers);
    }

    /// <summary>
    /// 获取K线数据
    /// </summary>
    /// <param name="symbol">交易对标识</param>
    /// <param name="interval">K线周期</param>
    /// <param name="limit">返回数量，默认100</param>
    /// <param name="startTime">开始时间戳（毫秒）</param>
    /// <param name="endTime">结束时间戳（毫秒）</param>
    /// <returns></returns>
    [HttpGet("candlestick/{symbol}/{interval}")]
    public async Task<IActionResult> GetCandlestick(
        string symbol,
        string interval,
        [FromQuery] int limit = 100,
        [FromQuery] long? startTime = null,
        [FromQuery] long? endTime = null)
    {
        var candlesticks = await _candlestickService.GetCandlesticksAsync(
            symbol, interval, limit, startTime, endTime);
        return Ok(candlesticks);
    }
}