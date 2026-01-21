

namespace Admin.NET.Core;

public class EventHandlerMonitor : IEventHandlerMonitor
{
    public Task OnExecutingAsync(EventHandlerExecutingContext context)
    {
        //_logger.LogInformation("执行之前：{EventId}", context.Source.EventId);
        return Task.CompletedTask;
    }

    public Task OnExecutedAsync(EventHandlerExecutedContext context)
    {
        //_logger.LogInformation("执行之后：{EventId}", context.Source.EventId);

        if (context.Exception != null)
        {
            Log.Error($"EventHandlerMonitor.OnExecutedAsync 执行出错啦：{context.Source.EventId}", context.Exception);
        }

        return Task.CompletedTask;
    }
}