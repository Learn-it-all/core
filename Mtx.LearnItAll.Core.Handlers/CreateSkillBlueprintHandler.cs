using MediatR;
using Mtx.Common.Domain;
using Mtx.CosmosDbServices;
using Mtx.CosmosDbServices.Entities;
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
			var id = UniqueId.New();
			var newBlueprint = SkillBluePrint.Create(id,request.Name);

			var addResult = await cosmosDb.TransactionalBatchAddAsync<SkillBluePrintEvent,DomainEvent>(newBlueprint.Applied, PartitionKeyValue.From(id.Value), cancellationToken);
			return CreateSkillBlueprintResult.FromResult(addResult, SkillBlueprintData.New(Guid.Parse(id), Guid.Parse(newBlueprint.AggregateId)));

		}

	}
	
}
