using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
	public class AddPartToSkillBlueprintHandler : IRequestHandler<AddPartCmd, AddPartResult>, IRequestHandler<AddMultiplePartsCmd, AddMultiplePartsResult>
	{
		private readonly ICosmosDbService cosmosDb;

		public AddPartToSkillBlueprintHandler(ICosmosDbService cosmosDb)
		{
			this.cosmosDb = cosmosDb;
		}

		public async Task<AddPartResult> Handle(AddPartCmd request, CancellationToken cancellationToken)
		{
			var result = await cosmosDb.GetAsync<SkillBlueprint>(id: request.BlueprintId, partitionKey: request.BlueprintId, cancellationToken);
			if(result.IsError)
			{
				return AddPartResult.FromResult(result.Result);
			}
			AddPartResult tryAddResult = AddPartResult.FailureForUnknownReason;

			if (result.IsSuccessAndHasValue)
			{
				var skill = result.Contents;
				if (result.Contents.TryAdd(request, out tryAddResult))
					await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken);
				return tryAddResult;
			}

			return result;
		}

		public async Task<AddMultiplePartsResult> Handle(AddMultiplePartsCmd request, CancellationToken cancellationToken)
		{
			var dataResult = await cosmosDb.GetAsync<SkillBlueprint>(request.BlueprintId, request.BlueprintId, cancellationToken);
			if (dataResult.IsSuccessAndHasValue)
			{
				var skill = dataResult.Contents;
				var result = skill.Add(request);
				if (!result.HasErrors)
				{

					var updateResult = await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken);
				
				}
				return result;

			}
			else
			{
				return new AddMultiplePartsResult()
			}


		}
	}
}
