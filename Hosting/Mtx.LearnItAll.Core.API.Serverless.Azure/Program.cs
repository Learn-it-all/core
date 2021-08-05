using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Mtx.CosmosDbServices;

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
                .ConfigureServices(async (hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<CosmosConfig>(hostContext.Configuration.GetSection("CosmosConfig"));
                    services.AddDbContext<CosmosDbContext>();
                    services.InitializeCosmosClientInstance(hostContext.Configuration);


                })
                .Build();

            await host.RunAsync();
        }
    }
}