using Mtx.CosmosDbServices.Entities;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core.Infrastructure.CosmosDbServices.Skills;

public record SkillBlueprintDocumentIdentity : ICosmosDocumentIdentity
{
	public SkillBlueprintDocumentIdentity(SkillBlueprint blueprint) : this(blueprint.Id) { }

	public SkillBlueprintDocumentIdentity(Guid id)
	{
		Id = id.ToString();
		PartitionKey = Id;
	}
	public string Id { get; }
	public object PartitionKey { get; }
}