using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Collections.Generic;

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
#pragma warning disable CS8618 
        private PartNode() { }
#pragma warning restore CS8618 


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
                Add(part);
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


       
        public void Add(PartNode newNode)
        {
            MakeSureNameIsNotInUseInPartNodes(newNode.Name);
            newNode.Summary.RaiseChangeEvent += Summary.RecalculateOnChange;
            _nodes.Add(newNode);
        }

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

       
    }
}