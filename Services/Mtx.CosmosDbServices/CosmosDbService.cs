using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Mtx.LearnItAll.Core.Blueprints;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.CosmosDbServices;


public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;
    private readonly IHttpContextAccessor _httpContext;

    public CosmosDbService(
        CosmosClient dbClient,
        string databaseName,
        string containerName,
        IHttpContextAccessor httpContext)
    {
        this._container = dbClient.GetContainer(databaseName, containerName);
        this._httpContext = httpContext;
    }

    public async Task AddAsync(SkillBlueprint skillBlueprint, CancellationToken cancellationToken = default(CancellationToken))
    {
        skillBlueprint.CreatedBy = _httpContext?.HttpContext?.User?.Identity?.Name ?? "anonymous user";

        var response = await this._container.CreateItemAsync(skillBlueprint, new PartitionKey(skillBlueprint.Id.ToString()), cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
        await this._container.DeleteItemAsync<SkillBlueprint>(id, new PartitionKey(id), cancellationToken: cancellationToken);
    }

    public async Task<SkillBlueprint> GetSkillBlueprintAsync(string id)
    {
        ItemResponse<SkillBlueprint> response = await this._container.ReadItemAsync<SkillBlueprint>(id, new PartitionKey(id));
        return response.Resource;

    }

    //SELECT * FROM c where STRINGEQUALS(c._root.name,'c#', true) performs insensitive string 
    public async Task<IEnumerable<SkillBlueprint>> GetItemAsync(string queryString)
    {
        var query = this._container.GetItemQueryIterator<SkillBlueprint>(new QueryDefinition(queryString));
        List<SkillBlueprint> results = new List<SkillBlueprint>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task UpdateAsync(string id, SkillBlueprint skillBlueprint)
    {
        skillBlueprint.ModifiedDate = System.DateTime.Now;
        skillBlueprint.ModifiedBy = _httpContext?.HttpContext?.User?.Identity?.Name ?? "anonymous user";
        var result = await this._container.UpsertItemAsync<SkillBlueprint>(skillBlueprint, new PartitionKey(id));
        var retries = 0;
        while (result.StatusCode == System.Net.HttpStatusCode.Conflict && retries++ < 3)
        {
            result = await this._container.UpsertItemAsync<SkillBlueprint>(skillBlueprint, new PartitionKey(id));
        }
    }
}
