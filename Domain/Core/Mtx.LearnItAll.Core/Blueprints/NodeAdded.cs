using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Common;
using Newtonsoft.Json;
using System;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public record NodeAdded : SkillBluePrintEvent
	{
		public NodeAdded(UniqueId aggregateId, UniqueId parentId, UniqueId id, Name name) : this(aggregateId, parentId, id, name, DateTimeOffset.Now) { }

		[JsonConstructor]
		private NodeAdded(string aggregateId, string parentId, string id, string name, DateTimeOffset validOn) : base(validOn, 0)
		{

			Name = name;
			Id = id;
			ParentId = parentId;
			AggregateId = aggregateId;
		}

		public string Name { get; }
		public string Id { get; }
		public string ParentId { get; }
		public string AggregateId { get; }

		public static NodeAdded With(UniqueId aggregateId, UniqueId parentId, UniqueId id, Name name) => new(aggregateId, parentId, id, name);
		public static NodeAdded With(UniqueId aggregateId, UniqueId parentId, UniqueId id, Name name, DateTimeOffset validOn) => new(aggregateId, parentId, id, name, validOn);

		public static DomainEvent From(UniqueId aggregateId, Node item) => With(aggregateId, UniqueId.From(item.ParentId), UniqueId.From(item.Id), item.Name.ToName());
	}
}