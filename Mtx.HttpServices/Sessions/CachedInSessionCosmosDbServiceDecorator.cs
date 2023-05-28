using Microsoft.AspNetCore.Http;
using Mtx.CosmosDbServices;
using Mtx.CosmosDbServices.Entities;
using Mtx.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.HttpServices.Sessions
{
    public class CachedInSessionCosmosDbServiceDecorator : ICosmosDbService
    {
        private readonly ICosmosDbService decorated;
        private readonly IHttpContextAccessor http;

        public CachedInSessionCosmosDbServiceDecorator(ICosmosDbService decorated, IHttpContextAccessor http)
        {
            if (http is null)
            {
                throw new ArgumentNullException(nameof(http));
            }

            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            this.http = http;
        }

		public async Task<Result> AddAsync<T>(T item, object id, object partitionKey, CancellationToken cancellationToken)
		{
			var result = await decorated.AddAsync(item, id, partitionKey, cancellationToken);
			if(result.IsSuccess)
				http.HttpContext.Session.SetString(id.ToString(), JsonConvert.SerializeObject(item));
			return result;
		}

		public async Task<Result> AddAsync<T>(T item, ICosmosDocumentIdentity identity, CancellationToken cancellationToken)
		{
			return await decorated.AddAsync(item, identity, cancellationToken);
		}

		public async Task<DataResult<T>> GetAsync<T>(object id, object partitionKey, CancellationToken ct = default)
		{
			return await decorated.GetAsync<T>(id, partitionKey, ct);
		}

		public async Task<DataResult<List<T>>> GetItemsAsync<T>(object query, CancellationToken cancellationToken)
		{
			return await decorated.GetItemsAsync<T>(query, cancellationToken);
		}

		public async Task<Result> UpdateAsync<T>(T item, object id, object partitionKey, CancellationToken ct)
		{
			return await decorated.UpdateAsync(item, id, partitionKey, ct);
		}

		public async Task<Result> UpdateAsync<T>(T item, ICosmosDocumentIdentity identity, CancellationToken ct)
		{
			return await decorated.UpdateAsync(item, identity, ct);
		}
	}
}
