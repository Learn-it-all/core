using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure
{
    public class TopLevelSkillApi
    {
        readonly CosmosConfig _cosmosConfig;
        readonly CosmosDbContext context;

        public TopLevelSkillApi(IOptions<CosmosConfig> options, CosmosDbContext context)
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

            TopLevelSkill entity = new (new ModelName("C#"));
            entity.Add(new SkillModel(new ModelName("delegates")));
#if DEBUG
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
# endif
            context.Add(entity);
            var items = context.SaveChanges();
            var ent = context.Set<TopLevelSkill>().Find(entity.Id);
            entity.Add(new SkillModel(new ModelName("Collections")));
            context.SaveChanges();
            ent = context.Set<TopLevelSkill>().Find(entity.Id);

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteAsJsonAsync(ent);

            return response;
        }
    }
}
