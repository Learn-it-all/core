using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure
{
    public class SkillBlueprintApi
    {
        readonly CosmosConfig _cosmosConfig;
        readonly CosmosDbContext context;
        private readonly ICosmosDbService _cosmos;

        public SkillBlueprintApi(IOptions<CosmosConfig> options, CosmosDbContext context, ICosmosDbService cosmos)
        {
            _cosmosConfig = options?.Value ?? throw new ArgumentException("param required", nameof(options));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this._cosmos = cosmos;
        }

        [Function("SkillBlueprintApi")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "skills")] HttpRequestData req,
            FunctionContext executionContext)
        {

            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");


            SkillBlueprint entity = new(new Name("C#"));
            PartNode keywordsNode = new PartNode(new Name("keywords"));
            keywordsNode.Add(new AddPartCmd(new Name("abstract"), keywordsNode.Id));
            entity.Add(keywordsNode);


            await _cosmos.AddAsync(entity);

            SkillBlueprint enti = await _cosmos.GetSkillBlueprintAsync(entity.Id.ToString());
            if (enti != null)
            {
                enti.Add(new AddPartCmd(new Name("override"), keywordsNode.Id));


                PartNode node = new PartNode(new Name("Collections"));
                node.Add(new AddPartCmd(new Name("List<T>"), node.Id));
                node.Add(new AddPartCmd(new Name("List"), node.Id));
                node.Add(new AddPartCmd(new Name("Set<T>"), node.Id));
                node.Add(new AddPartCmd(new Name("HashTable<T>"), node.Id));

                enti.Add(node);

                await _cosmos.UpdateAsync(entity.Id.ToString(), enti);
            }

            enti = await _cosmos.GetSkillBlueprintAsync(enti?.Id.ToString() ?? "");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(enti);
            return await Task.FromResult(response);

        }

        [Function("SkillBlueprintApi_post")]
        public HttpResponseData RunPost([HttpTrigger(AuthorizationLevel.Function, "post", Route = "skills")] HttpRequestData req,
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
