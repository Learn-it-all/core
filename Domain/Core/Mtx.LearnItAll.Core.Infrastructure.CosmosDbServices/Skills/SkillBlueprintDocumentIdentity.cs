using Microsoft.Azure.Cosmos;
using Mtx.CosmosDbServices.Entities;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core.Infrastructure.CosmosDbServices.Skills;

public record SkillBlueprintDocumentIdentity : ICosmosDocumentIdentity
{
	public SkillBlueprintDocumentIdentity(SkillBlueprint blueprint) : this(blueprint.Id) { }

	public SkillBlueprintDocumentIdentity(Guid id)
	{
		CosmosId = id.ToString();
		PartitionKey = new(CosmosId);
	}
	public string CosmosId { get; }
	public PartitionKey PartitionKey { get; }
}