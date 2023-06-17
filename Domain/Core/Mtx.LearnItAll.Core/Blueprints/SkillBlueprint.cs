using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public class SkillBlueprint : Entity
	{

#pragma warning disable CS8618
		protected SkillBlueprint()
#pragma warning restore CS8618
		{
		}

		public SkillBlueprint(Name name)
		{
			_root = new PartNode(name, Id);
			CreatedDate = DateTime.Now;
		}

		[JsonIgnore]
		public LifecycleState LifecycleState => _root.LifecycleState;

		[JsonIgnore]
		public Guid RootPartId => _root.Id;

		public string Name => _root.Name;

		[JsonIgnore]
		public IReadOnlyCollection<PartNode> Nodes => _root.Nodes;

		[JsonIgnore]
		public IReadOnlyCollection<Part> Parts => _root.Parts;

		[JsonIgnore]
		public Summary Summary => _root.Summary.Copy();

		[JsonProperty(PropertyName = "root")]
		private readonly PartNode _root;

		public void Add(PartNode skill)
		{
			_root.Add(skill);
		}
		public void Add(AddPartCmd cmd)
		{
			_root.Add(cmd);
		}

		public bool TryAdd(AddPartCmd cmd, out AddPartResult result)
		{
			return _root.TryAdd(cmd, out result);
		}
		public AddMultiplePartsResult Add(AddMultiplePartsCmd cmd)
		{

			var finalResult = AddMultiplePartsResult.Ok200();
			var result = AddPartResult.FailureForUnknownReason;
			foreach (var name in cmd.Names)
			{
				_root.TryAdd(new AddPartCmd(name, cmd.ParentId, Id), out result);
				if (result == AddPartResult.FailureForParentNodeNotFound) return new AddMultiplePartsResultNoParentNodeFound(cmd.Names);
				finalResult.Add(name, result);
			}

			return finalResult;
		}
		public bool TryDeletePart(DeletePartCmd cmd, out DeletePartResult result)
		{
			return _root.TryDeletePart(cmd, out result);
		}
	}

	public record UniqueId
	{
		private UniqueId()
		{
			Value = Guid.NewGuid().ToString();
		}

		private UniqueId(string provided)
		{
			if (string.IsNullOrEmpty(provided))
			{
				throw new ArgumentException($"'{nameof(provided)}' cannot be null or empty.", nameof(provided));
			}
			Value = provided;
		}

		public string Value { get; }
		public static UniqueId New() => new UniqueId();
		public static UniqueId From(string provided) => new UniqueId(provided);
		public static implicit operator string(UniqueId id) => id.Value;
	}

	public class Node
	{
		private readonly List<Node> _nodes = new List<Node>();
		public Node(UniqueId id, Name name, UniqueId parentId)
		{
			Name = name;
			ParentId = parentId;
			Id = id;
		}

		public void Add(Node node)
		{
			_nodes.Add(node);
		}

		public string Name { get; }
		public string ParentId { get; }
		public string Id { get; }
	}

	public record SkillCreated : DomainEvent
	{
		public SkillCreated(UniqueId id, Name name) : this(id, name, DateTimeOffset.Now) { }

		[JsonConstructor]
		private SkillCreated(string id, string name, DateTimeOffset validOn) : base(validOn, 0)
		{

			Name = name;
			Id = id;

		}

		public string Name { get; }
		public string Id { get; }
		//CosmosDbPArtitionKey
		public string AggregateId => Id;

		public static SkillCreated With(UniqueId id, Name name) => new(id, name);
		public static SkillCreated With(UniqueId id, Name name, DateTimeOffset validOn) => new(id, name, validOn);
	}

	public record NodeAdded : DomainEvent
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

	public class Skill : SourcedEntity<DomainEvent>
	{
		public List<Node> _nodes = new();
		private Dictionary<string, Node> _nodesByName = new();
		private Dictionary<string, Node> _nodesById = new();

		public string AggregateId { get; private set; }
		public string Name { get; private set; }

		public static Skill Create(UniqueId id, Name name) => new(id, name);
		public static Skill Create(UniqueId id, Name name, DateTimeOffset validOn) => new(id, name, validOn);

		public Skill(UniqueId id, Name name) => Apply(SkillCreated.With(id, name));
		public Skill(UniqueId id, Name name, DateTimeOffset validOn) => Apply(SkillCreated.With(id, name, validOn));
		public Skill(IEnumerable<DomainEvent> eventStream, int streamVersion) : base(eventStream, streamVersion) { }

		public void When(SkillCreated e)
		{
			Name = e.Name;
			AggregateId = e.Id;
		}

		public void When(NodeAdded e)
		{
			Node item = new Node(UniqueId.From(e.Id), new Name(e.Name), UniqueId.From(e.ParentId));
			TryAddNoSideEffects(item, out _);
		}

		public bool TryAdd(Node newNode, out Result result)
		{
			if (TryAddNoSideEffects(newNode, out result))
			{
				Apply(NodeAdded.From(aggregateId: UniqueId.From(AggregateId), item: newNode));
				return true;
			}
			return false;

		}

		private bool TryAddNoSideEffects(Node newNode, out Result result)
		{
			try
			{
				if (newNode.ParentId.Equals(AggregateId))
				{
					if (_nodesByName.TryGetValue(newNode.Name, out _))
					{
						result = new Result(StatusCodes.Status409Conflict);
						return false;
					}

					_nodes.Add(newNode);
				}
				else
				{
					if (!_nodesById.TryGetValue(newNode.Id, out var parent))
					{
						result = Result.BadRequest400("Node not found");
						return false;
					}

					parent.Add(newNode);

				}
				_nodesByName[newNode.Name] = newNode;
				_nodesById[newNode.Id] = newNode;
				result = Result.NoContent204();
				return true;

			}
			catch (Exception e)
			{
				result = Result.InternalErrorWithGenericErrorMessage(e);
				return false;

			}
		}

		public static Skill Recreate(IEnumerable<DomainEvent> events, int streamVersion) => new(events, streamVersion);
	}
}