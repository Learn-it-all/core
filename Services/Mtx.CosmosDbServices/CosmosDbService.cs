using System;

namespace Mtx.CosmosDbServices
{
        using System.Collections.Generic;
        using System.Linq;
        using System.Threading.Tasks;
        using Microsoft.Azure.Cosmos;
        using Microsoft.Azure.Cosmos.Fluent;
        using Microsoft.Extensions.Configuration;
        using Mtx.LearnItAll.Core.Blueprints;

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddAsync(SkillBlueprint skillBlueprint)
        {
            await this._container.CreateItemAsync(skillBlueprint,new PartitionKey(skillBlueprint.Id.ToString()));
        }

        public async Task DeleteAsync(string id)
        {
            await this._container.DeleteItemAsync<SkillBlueprint>(id, new PartitionKey(id));
        }

        public async Task<SkillBlueprint> GetSkillBlueprintAsync(string id)
        {
            ItemResponse<SkillBlueprint> response = await this._container.ReadItemAsync<SkillBlueprint>(id, new PartitionKey(id));
            return response.Resource;

        }

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
            await this._container.UpsertItemAsync<SkillBlueprint>(skillBlueprint, new PartitionKey(id));
        }
    }
}
