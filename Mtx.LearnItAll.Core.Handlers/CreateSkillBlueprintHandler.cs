using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Threading;
using System.Threading.Tasks;

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
			var result = await cosmosDb.AddAsync(newBlueprint, newBlueprint.Id, newBlueprint.Id, cancellationToken);
			return CreateSkillBlueprintResult.FromResult(result, SkillBlueprintData.New(newBlueprint.Id, newBlueprint.RootPartId));

		}

	}
}
