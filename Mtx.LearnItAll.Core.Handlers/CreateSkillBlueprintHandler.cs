using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Blueprints.SharedKernel;
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
			var queryResult = await cosmosDb.CountAsync<IsSkillBluePrintNameInUse>(new(new(request.Name)), cancellationToken);
			if (queryResult.IsError)
				return CreateSkillBlueprintResult.FromResult(queryResult.Result);
			var counter = queryResult.Contents;
			if (counter.Some)
				return CreateSkillBlueprintResult.Conflict409();

			var newBlueprint = PersonalSkill.Create(request.Name);
			var addResult = await cosmosDb.AddUsingIdAsPartitionKeyAsync(newBlueprint, cancellationToken);
			return CreateSkillBlueprintResult.FromResult(addResult, SkillBlueprintData.New(newBlueprint.Id, newBlueprint.RootPartId));

		}

	}
	
}
