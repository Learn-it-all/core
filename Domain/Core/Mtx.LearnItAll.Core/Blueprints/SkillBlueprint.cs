using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Common;
using Mtx.Results;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public class SkillBluePrint : SourcedEntity<DomainEvent>
	{
		public List<Node> _nodes = new();
		private Dictionary<string, Node> _nodesByName = new();
		private Dictionary<string, Node> _nodesById = new();

		public string AggregateId { get; private set; }
		public string Name { get; private set; }

		public static SkillBluePrint Create(UniqueId id, Name name) => new(id, name);
		public static SkillBluePrint Create(UniqueId id, Name name, DateTimeOffset validOn) => new(id, name, validOn);

		public SkillBluePrint(UniqueId id, Name name) => Apply(SkillCreated.With(id, name));
		public SkillBluePrint(UniqueId id, Name name, DateTimeOffset validOn) => Apply(SkillCreated.With(id, name, validOn));
		public SkillBluePrint(IEnumerable<DomainEvent> eventStream, int streamVersion) : base(eventStream, streamVersion) { }

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

		public static SkillBluePrint Recreate(IEnumerable<DomainEvent> events, int streamVersion) => new(events, streamVersion);
	}
}