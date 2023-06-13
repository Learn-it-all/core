using MediatR;
using Mtx.CosmosDbServices;
using Mtx.CosmosDbServices.Entities;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;

namespace Mtx.LearnItAll.Core.Handlers;

public class DeleteSkillBlueprintHandler : IRequestHandler<DeleteBlueprintCmd, DeleteBlueprintResult>
{
	private readonly ICosmosDbService cosmosDb;

	public DeleteSkillBlueprintHandler(ICosmosDbService cosmosDb)
	{
		this.cosmosDb = cosmosDb;
	}

	public async Task<DeleteBlueprintResult> Handle(DeleteBlueprintCmd request, CancellationToken cancellationToken)
	{
			var result = await cosmosDb.DeleteUsingIdAsPartitionKeyAsync<SkillBlueprint>(DocumentId.From(request.BlueprintId),cancellationToken);
			return await Task.FromResult(DeleteBlueprintResult.FromResult(result));
	}

}
