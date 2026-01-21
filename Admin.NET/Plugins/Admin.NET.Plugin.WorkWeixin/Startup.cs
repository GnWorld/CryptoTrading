

using Admin.NET.Plugin.WorkWeixin.Option;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.NET.Plugin.WorkWeixin;

[AppStartup(100)]
public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfigurableOptions<WorkWeixinOptions>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}