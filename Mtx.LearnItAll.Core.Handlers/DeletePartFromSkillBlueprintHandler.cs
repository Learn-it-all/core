using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Threading;
using System.Threading.Tasks;

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
		var dataResult = await cosmosDb.GetAsync<SkillBlueprint>(request.BlueprintId, cancellationToken);
		if (dataResult.IsError)
		{
			return DeletePartResult.FromResult(dataResult.Result);
		}
		var skill = dataResult.Contents;
		if (skill.TryDeletePart(request, out DeletePartResult result))
		{
			var updateResult = await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken);
			if(updateResult.IsError)
			{
				return DeletePartResult.FromResult(dataResult.Result);

			}
		}

		return result;
	}

}