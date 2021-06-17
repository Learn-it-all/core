using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure
{
    public class Program
    {
        static async Task Main()
        {
#if DEBUG
            Debugger.Launch();
#endif
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                 .ConfigureAppConfiguration((hostContext, builder) =>
                 {
                     if (hostContext.HostingEnvironment.IsDevelopment())
                     {
                         builder.AddUserSecrets<Program>();
                     }
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<CosmosConfig>(hostContext.Configuration.GetSection("CosmosConfig"));
                    services.AddDbContext<CosmosDbContext>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}