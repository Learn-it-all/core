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
		var skill = await cosmosDb.GetAsync<SkillBlueprint>(request.BlueprintId, cancellationToken);
		if (skill.TryDeletePart(request, out DeletePartResult result))
			await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken);

		return result;
	}

}