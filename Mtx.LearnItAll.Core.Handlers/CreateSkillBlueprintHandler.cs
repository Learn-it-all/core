using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;

namespace Mtx.LearnItAll.Core.Handlers
{
	public class CreateSkillBlueprintHandler : IRequestHandler<CreateSkillBlueprintCmd, CreateSkillBlueprintResult>
	{
		private readonly ICosmosDbService cosmosDb;

		public CreateSkillBlueprintHandler(ICosmosDbService cosmosDb)
		{
			this.cosmosDb = cosmosDb;
		}

		public async Task<CreateSkillBlueprintResult> Handle(CreateSkillBlueprintCmd request, CancellationToken cancellationToken)
		{
			var newBlueprint = new SkillBlueprint(request.Name);
			var result = await cosmosDb.AddUsingIdAsPartitionKeyAsync(newBlueprint, cancellationToken);
			return CreateSkillBlueprintResult.FromResult(result, SkillBlueprintData.New(newBlueprint.Id, newBlueprint.RootPartId));

		}

	}
}
