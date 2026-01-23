// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace Admin.NET.Application.Services;

using Admin.NET.Application.Market.Services;
using Admin.NET.Application.Services.Market.Models;
using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


[JobDetail("job_KlineConsumerJob", Description = "K线行情消费", GroupName = "default", Concurrent = false)]
[PeriodSeconds(1, TriggerId = "trigger_KlineConsumerJob", Description = "K线行情消费", MaxNumberOfRuns = 1, RunOnStart = true)]
public class KlineConsumerJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KlineConsumerJob> _logger;

    public KlineConsumerJob(
        IServiceScopeFactory scopeFactory,
        ILogger<KlineConsumerJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kline consumer job started");

        // ⚠️ Run 类型 Job 只会执行一次，所以这里要 while 常驻
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var evt = await KlineChannel.Channel.Reader
                    .ReadAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider
                    .GetRequiredService<CandlestickService>();

                await service.HandleKlineUpdateAsync(
                    evt.Symbol,
                    evt.Interval,
                    evt.Data);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kline consumer error");
            }
        }
    }
}
