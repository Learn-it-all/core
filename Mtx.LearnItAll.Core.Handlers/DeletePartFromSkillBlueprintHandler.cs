using MediatR;
using Mtx.CosmosDbServices;
using Mtx.CosmosDbServices.Entities;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;

namespace Mtx.LearnItAll.Core.Handlers;
public class DeletePartFromSkillBlueprintHandler : IRequestHandler<DeletePartCmd, DeletePartResult>
{
	private readonly ICosmosDbService cosmosDb;

	public DeletePartFromSkillBlueprintHandler(ICosmosDbService cosmosDb)
	{
		this.cosmosDb = cosmosDb;
	}

	public async Task<DeletePartResult> Handle(DeletePartCmd request, CancellationToken cancellationToken)
	{
		var dataResult = await cosmosDb.GetUsingIdAsPartitionKeyAsync<PersonalSkill>(DocumentId.From(request.BlueprintId), cancellationToken);
		if (dataResult.IsError)
		{
			return DeletePartResult.FromResult(dataResult.Result);
		}
		var skill = dataResult.Contents;
		if (skill.TryDeletePart(request, out DeletePartResult result))
		{
			var updateResult = await cosmosDb.UpdateUsingIdAsPartitionKeyAsync(skill, cancellationToken);
			if (updateResult.IsError)
			{
				return DeletePartResult.FromResult(dataResult.Result);

			}
		}

		return result;
	}

}