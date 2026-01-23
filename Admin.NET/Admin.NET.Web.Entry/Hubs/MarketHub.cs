using Admin.NET.Application.Market.Models;
using Admin.NET.Application.Market.Services;
using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Web.Entry.Hubs;

/// <summary>
/// 行情Hub
/// </summary>
public class MarketHub : Hub
{
    private readonly MarketSnapshotService _marketSnapshotService;
    private static readonly Dictionary<string, List<string>> _symbolSubscribers = new();
    private static readonly List<string> _allMarketSubscribers = new();

    public MarketHub(MarketSnapshotService marketSnapshotService)
    {
        _marketSnapshotService = marketSnapshotService;
    }

    /// <summary>
    /// 订阅单个交易对行情
    /// </summary>
    /// <param name="symbol">交易对标识</param>
    /// <returns></returns>
    public async Task SubscribeSymbol(string symbol)
    {
        if (!_symbolSubscribers.ContainsKey(symbol))
        {
            _symbolSubscribers[symbol] = new List<string>();
        }
        
        if (!_symbolSubscribers[symbol].Contains(Context.ConnectionId))
        {
            _symbolSubscribers[symbol].Add(Context.ConnectionId);
        }
        
        // 立即推送当前行情
        var snapshot = _marketSnapshotService.GetOrAdd(symbol);
        if (snapshot != null)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("SymbolUpdate", snapshot);
        }
    }

    /// <summary>
    /// 取消订阅单个交易对行情
    /// </summary>
    /// <param name="symbol">交易对标识</param>
    /// <returns></returns>
    public Task UnsubscribeSymbol(string symbol)
    {
        if (_symbolSubscribers.ContainsKey(symbol))
        {
            _symbolSubscribers[symbol].Remove(Context.ConnectionId);
            if (_symbolSubscribers[symbol].Count == 0)
            {
                _symbolSubscribers.Remove(symbol);
            }
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 订阅所有交易对行情
    /// </summary>
    /// <returns></returns>
    public async Task SubscribeAllMarket()
    {
        if (!_allMarketSubscribers.Contains(Context.ConnectionId))
        {
            _allMarketSubscribers.Add(Context.ConnectionId);
        }
        
        // 立即推送所有当前行情
        var snapshots = _marketSnapshotService.GetAllSnapshots();
        await Clients.Client(Context.ConnectionId).SendAsync("AllMarketUpdate", snapshots);
    }

    /// <summary>
    /// 取消订阅所有交易对行情
    /// </summary>
    /// <returns></returns>
    public Task UnsubscribeAllMarket()
    {
        _allMarketSubscribers.Remove(Context.ConnectionId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 连接断开时清理订阅
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
        // 清理单个交易对订阅
        foreach (var symbol in _symbolSubscribers.Keys.ToList())
        {
            _symbolSubscribers[symbol].Remove(Context.ConnectionId);
            if (_symbolSubscribers[symbol].Count == 0)
            {
                _symbolSubscribers.Remove(symbol);
            }
        }
        
        // 清理所有行情订阅
        _allMarketSubscribers.Remove(Context.ConnectionId);
        
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 推送单个交易对行情更新
    /// </summary>
    /// <param name="snapshot">行情快照</param>
    /// <returns></returns>
    public static async Task PushSymbolUpdate(IHubContext<MarketHub> hubContext, FuturesMarketSnapshot snapshot)
    {
        if (_symbolSubscribers.ContainsKey(snapshot.Symbol))
        {
            await hubContext.Clients.Clients(_symbolSubscribers[snapshot.Symbol])
                .SendAsync("SymbolUpdate", snapshot);
        }
    }

    /// <summary>
    /// 推送所有交易对行情更新
    /// </summary>
    /// <param name="snapshots">行情快照列表</param>
    /// <returns></returns>
    public static async Task PushAllMarketUpdate(IHubContext<MarketHub> hubContext, IEnumerable<FuturesMarketSnapshot> snapshots)
    {
        if (_allMarketSubscribers.Count > 0)
        {
            await hubContext.Clients.Clients(_allMarketSubscribers)
                .SendAsync("AllMarketUpdate", snapshots);
        }
    }
}