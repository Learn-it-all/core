using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Common;
using Newtonsoft.Json;
using System;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public record SkillBlueprintCreated : SkillBluePrintEvent
	{
		public SkillBlueprintCreated(UniqueId id, Name name) : this(id, name, DateTimeOffset.Now) { }

		[JsonConstructor]
		private SkillBlueprintCreated(string id, string name, DateTimeOffset validOn) : base(validOn, 0)
		{

			Name = name;
			Id = id;

		}

		public string Name { get; }
		public string Id { get; }
		//CosmosDb PartitionKey
		public string AggregateId => Id;

		public static SkillBlueprintCreated With(UniqueId id, Name name) => new(id, name);
		public static SkillBlueprintCreated With(UniqueId id, Name name, DateTimeOffset validOn) => new(id, name, validOn);
	}
}