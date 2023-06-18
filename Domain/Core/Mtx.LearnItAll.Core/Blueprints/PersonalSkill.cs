using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public class PersonalSkill : Entity
	{

#pragma warning disable CS8618
		protected PersonalSkill()
#pragma warning restore CS8618
		{
		}
		public static PersonalSkill Create(Name name) => new PersonalSkill(name);
		private PersonalSkill(Name name)
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
}