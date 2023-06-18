using Mtx.LearnItAll.Core.Common;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
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
}