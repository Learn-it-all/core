using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure
{
    public class TopLevelSkillApi
    {
        readonly CosmosConfig _cosmosConfig;
        readonly CoreDbContext context;

        public TopLevelSkillApi(IOptions<CosmosConfig> options, CoreDbContext context)
        {
            _cosmosConfig = options?.Value ?? throw new ArgumentException("param required", nameof(options));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [Function("TopLevelSkillS")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            context.Add(new TopLevelSkill(new ModelName("C#")));
            var items = context.SaveChanges();

            logger.LogError("items added: ", items);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
