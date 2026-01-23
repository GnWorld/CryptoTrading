using Admin.NET.Application.Market.Models;
using Admin.NET.Application.Market.Services;
using Admin.NET.Web.Entry.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Admin.NET.Web.Entry.Services;

/// <summary>
/// 行情推送服务
/// </summary>
public class MarketPushService : IHostedService, IDisposable
{
    private readonly IHubContext<MarketHub> _hubContext;
    private readonly MarketSnapshotService _marketSnapshotService;
    private Timer _timer;

    public MarketPushService(
        IHubContext<MarketHub> hubContext,
        MarketSnapshotService marketSnapshotService)
    {
        _hubContext = hubContext;
        _marketSnapshotService = marketSnapshotService;
    }

    /// <summary>
    /// 启动服务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 设置定时器，定期推送行情数据
        _timer = new Timer(PushMarketData, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
        return Task.CompletedTask;
    }

    /// <summary>
    /// 停止服务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 推送行情数据
    /// </summary>
    /// <param name="state"></param>
    private void PushMarketData(object state)
    {
        try
        {
            var snapshots = _marketSnapshotService.GetAllSnapshots();
            if (snapshots.Any())
            {
                // 推送所有行情数据
                _hubContext.Clients.All.SendAsync("AllMarketUpdate", snapshots).Wait();
                
                // 推送单个行情数据
                foreach (var snapshot in snapshots)
                {
                    _hubContext.Clients.All.SendAsync("SymbolUpdate", snapshot).Wait();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"推送行情数据失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        _timer?.Dispose();
    }
}