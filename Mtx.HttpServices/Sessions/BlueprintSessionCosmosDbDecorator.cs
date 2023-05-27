using Microsoft.AspNetCore.Http;
using Mtx.CosmosDbServices;
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

		public async Task AddAsync<T>(T item, object partitionKey, CancellationToken cancellationToken)
		{
			await decorated.AddAsync(item, partitionKey, cancellationToken);
			//http.HttpContext.Session.SetString(item.Id.ToString(), JsonConvert.SerializeObject(skillBlueprint));

		}

		public Task<T?> GetAsync<T>(object id, CancellationToken ct = default)
		{
			return decorated.GetAsync<T>(id, ct);
		}

		public Task<List<T>> GetItemsAsync<T>(object query, CancellationToken cancellationToken)
		{
			return decorated.GetItemsAsync<T>(query, cancellationToken);
		}

		public Task UpdateAsync<T>(T item, object id, object partitionKey, CancellationToken ct)
		{
			return decorated.UpdateAsync(item, id, partitionKey, ct);
		}
	}
}
