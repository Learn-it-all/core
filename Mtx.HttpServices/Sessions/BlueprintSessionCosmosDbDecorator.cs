using Microsoft.AspNetCore.Http;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.HttpServices.Sessions
{
    public class BlueprintSessionCosmosDbDecorator : ICosmosDbService
    {
        private readonly ICosmosDbService decorated;
        private readonly IHttpContextAccessor http;

        public BlueprintSessionCosmosDbDecorator(ICosmosDbService decorated, IHttpContextAccessor http)
        {
            if (http is null)
            {
                throw new ArgumentNullException(nameof(http));
            }

            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            this.http = http;
        }

        public async Task AddAsync(SkillBlueprint skillBlueprint, CancellationToken cancellationToken = default)
        {
            await decorated.AddAsync(skillBlueprint, cancellationToken);
            http.HttpContext.Session.SetString(skillBlueprint.Id.ToString(), JsonConvert.SerializeObject(skillBlueprint));
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await decorated.DeleteAsync(id, cancellationToken);

        }

        public async Task<IEnumerable<SkillBlueprint>> GetItemAsync(string queryString)
        {
            return await decorated.GetItemAsync(queryString);
        }

        public async Task<SkillBlueprint> GetSkillBlueprintAsync(string id)
        {
            return await decorated.GetSkillBlueprintAsync(id);
        }

        public async Task UpdateAsync(string id, SkillBlueprint skillBlueprint)
        {
            await decorated.UpdateAsync(id, skillBlueprint);
            http.HttpContext.Session.SetString(skillBlueprint.Id.ToString(), JsonConvert.SerializeObject(skillBlueprint));
        }
    }
}
