
namespace Admin.NET.Application.Market.Services;

using Admin.NET.Application.Market.Models;
using System.Collections.Concurrent;

/// <summary>
/// 行情内存存储服务（交易系统唯一行情来源）
/// </summary>
public class MarketSnapshotService : IScoped
{
    private static readonly ConcurrentDictionary<string, FuturesMarketSnapshot> _snapshots = new();

    /// <summary>
    /// 更新行情快照
    /// </summary>
    public void Update(FuturesMarketSnapshot snapshot)
    {
        _snapshots.AddOrUpdate(snapshot.Symbol, snapshot, (_, __) => snapshot);
    }

    /// <summary>
    /// 获取最新行情
    /// </summary>
    public FuturesMarketSnapshot? GetOrAdd(string symbol)
    {
        var snapshot = _snapshots.GetOrAdd(symbol, new FuturesMarketSnapshot { Symbol = symbol });
        return snapshot;
    }
}

