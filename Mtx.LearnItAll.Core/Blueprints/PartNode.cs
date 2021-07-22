using Mtx.LearnItAll.Core.Resources;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{


    /// <summary>
    /// Represents a Skill PartNode and all the elements (other <see cref="Part"/>s) that might be necessary to completely describe it.
    /// </summary>
    public class PartNode
    {
        private readonly List<PartNode> _nodes = new();
        private readonly List<Part> _parts = new();

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ParentId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; private set; } = LifecycleState.Current;
        public IReadOnlyCollection<PartNode> Nodes { get => _nodes; }
        public IReadOnlyCollection<Part> Parts { get => _parts; }

        public PartNode(Name name)
        {
            Name = name;
        }
        public PartNode(Name name, Guid parentId)
        {
            Name = name;
            ParentId = parentId;
        }

        /// <summary>
        /// For EF Core
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private PartNode() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Add a <see cref="Part"/> as a direct child.
        /// </summary>
        /// <param name="skill">the Skill to be added a direct child</param>
        /// <exception cref="InvalidOperationException">When the name of the <paramref name="skill"/> already exists as a direct child of current object.</exception>
        public void Add(PartNode skill)
        {
            MakeSureSkillNameIsNotInUse(skill.Name);
            _nodes.Add(skill);
        }

        private void MakeSureSkillNameIsNotInUse(string name)
        {
            if (_nodes.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                var errorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, name, Name);
                throw new InvalidOperationException(errorMessage);
            }
        }

        /// <summary>
        /// Adds the <paramref name="skill"/> to any node in the graph that matches the <paramref name="parentId"/>
        /// </summary>
        /// <param name="parentId">The parent under which the <paramref name="skill"/> will be added to</param>
        /// <param name="skill">The <see cref="Part"/> to be added</param>
        /// <returns>true when the <paramref name="parentId"/> was found and the <paramref name="skill"/> added as its child. False when no parent with <paramref name="parentId"/> was found.</returns>
        /// <exception cref="InvalidOperationException">When the name of the <paramref name="skill"/>already exists as a direct child of <paramref name="parentId"/></exception>
        public bool TryAdd(Guid parentId, PartNode skill)
        {
            if (Id == parentId)
            {
                Add(skill);
                return true;
            }
            foreach (var child in _nodes)
                if (child.TryAdd(parentId, skill))
                    return true;
            return false;
        }
    }
}