using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mtx.LearnItAll.Core.Blueprints
{


    /// <summary>
    /// Represents a Skill PartNode and all the elements (other <see cref="Part"/>s and <see cref="PartNode"/>s) that might be necessary to completely describe it.
    /// </summary>
    public class PartNode
    {
        private readonly List<PartNode> _nodes = new();
        private readonly List<Part> _parts = new();

        public Guid ParentId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; private set; } = LifecycleState.Current;
        public IReadOnlyCollection<PartNode> Nodes { get => _nodes; }
        public IReadOnlyCollection<Part> Parts { get => _parts; }
        public Summary Summary { get; private set; } = new Summary();
        public Guid Id { get; private set; } = Guid.NewGuid();

        public void Add(AddPartCmd cmd)
        {
            if (cmd.ParentId == Id)
            {
                MakeSureNameIsNotInUseInParts(cmd.Name);
                var newPart = new Part(cmd.Name, cmd.ParentId);
                _parts.Add(newPart);
                Summary.AddOneTo(newPart.Level);
                return;
            }
            var part = _parts.Find(x => x.Id == cmd.ParentId);
            if (part != null)
            {
                PartNode newNode = part;
                _nodes.Add(newNode);
                _parts.Remove(part);
            }
            _nodes.ForEach(x => x.Add(cmd));

        }

        public void ChangeLevel(string partName, Guid parentId, int newLevel)
        {
            if (parentId != Id)
            {
                _nodes.ForEach(x => x.ChangeLevel(partName, parentId, newLevel));
            }
            var part = _parts.Find(x => x.Name.Equals(partName, StringComparison.OrdinalIgnoreCase));
            if (part != null)
            {
                var currentLevel = part.Level;
                part.ChangeLevel(newLevel);
                Summary.AddOneTo(newLevel);
                Summary.SubtractOneFrom(currentLevel);
            }
        }


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
        /// Add a <see cref="PartNode"/> as a direct child.
        /// </summary>
        /// <param name="skill">the Skill to be added a direct child</param>
        /// <exception cref="InvalidOperationException">When the name of the <paramref name="skill"/> already exists as a direct child of current object.</exception>
        public void Add(PartNode skill)
        {
            MakeSureNameIsNotInUseInPartNodes(skill.Name);
            _nodes.Add(skill);
        }

        private void MakeSureNameIsNotInUseInPartNodes(string name)
        {
            if (_nodes.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ThrowErrorForDuplicateName(name);
        }

        private void MakeSureNameIsNotInUseInParts(string name)
        {
            if (_parts.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ThrowErrorForDuplicateName(name);
        }

        private void ThrowErrorForDuplicateName(string name)
        {
            var errorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, name, Name);
            throw new InvalidOperationException(errorMessage);
        }

        /// <summary>
        /// Adds the <paramref name="skill"/> to any node in the graph that matches the <paramref name="parentId"/>
        /// </summary>
        /// <param name="parentId">The parent under which the <paramref name="skill"/> will be added to</param>
        /// <param name="skill">The <see cref="Part"/> to be added</param>
        /// <returns>true when the <paramref name="parentId"/> was found and the <paramref name="skill"/> added as its child. False when no parent with <paramref name="parentId"/> was found.</returns>
        public bool TryAdd(Guid parentId, PartNode partNode)
        {
            try
            {
                if (Id == parentId)
                {
                    Add(partNode);
                    return true;
                }
                foreach (var child in _nodes)
                    if (child.TryAdd(parentId, partNode))
                        return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}