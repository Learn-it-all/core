using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure
{
    public class SkillBlueprintApi
    {
        readonly CosmosConfig _cosmosConfig;
        readonly CosmosDbContext context;

        public SkillBlueprintApi(IOptions<CosmosConfig> options, CosmosDbContext context)
        {
            _cosmosConfig = options?.Value ?? throw new ArgumentException("param required", nameof(options));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [Function("SkillBlueprintApi")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "skillblueprints/{name}")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            SkillBlueprint entity = new (new Name("C#"));
#if DEBUG
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
# endif
            entity.Add(new PartNode(new Name("delegates")));
            context.Add(entity);
            var items = context.SaveChanges();
            var ent = context.Set<SkillBlueprint>().Find(entity.Id);
            entity.Add(new PartNode(new Name("Collections")));
            context.SaveChanges();
            ent = context.Set<SkillBlueprint>().Find(entity.Id);

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteAsJsonAsync(ent);

            return response;
        }

        [Function("SkillBlueprintApi_post")]
        public HttpResponseData RunPost([HttpTrigger(AuthorizationLevel.Function, "post",Route = "skillblueprints")] HttpRequestData req,
           FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            SkillBlueprint entity = new(new Name("C#"));
#if DEBUG
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
# endif
            entity.Add(new PartNode(new Name("delegates")));
            context.Add(entity);
            var items = context.SaveChanges();
            var ent = context.Set<SkillBlueprint>().Find(entity.Id);
            entity.Add(new PartNode(new Name("Collections")));
            context.SaveChanges();
            ent = context.Set<SkillBlueprint>().Find(entity.Id);

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteAsJsonAsync(ent);

            return response;
        }
    }
}
