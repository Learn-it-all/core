﻿using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Mtx.LearnItAll.Core.Blueprints
{


    /// <summary>
    /// Represents a SkillBluePrint PartNode and all the elements (other <see cref="Part"/>s and <see cref="PartNode"/>s) that might be necessary to completely describe it.
    /// </summary>
    public class PartNode
    {
        private ObservableCollection<PartNode> _partNodes = new();

        public Guid ParentId { get; private set; }
        [JsonIgnore]
        public bool IsEmpty => _partNodes.Count == 0 & _parts.Count == 0;
        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; private set; } = LifecycleState.Draft;
        public IReadOnlyCollection<PartNode> Nodes => _partNodes;
        public IReadOnlyCollection<Part> Parts => _parts;
        private List<Part> _parts = new();
        public Summary Summary { get; private set; } = new Summary();
        public Guid Id { get; private set; } = Guid.NewGuid();

        public PartNode(Name name) : this()
        {
            Name = name;
        }
        public PartNode(Name name, Guid parentId) : this()
        {
            Name = name;
            ParentId = parentId;
        }

        public PartNode(Part originalPart) : this(new Name(originalPart.Name), originalPart.ParentId)
        {
            Id = originalPart.Id;
        }

        /// <summary>
        /// For deserialization
        /// </summary>
#pragma warning disable CS8618
        private PartNode()
        {
            _partNodes.CollectionChanged += PartNodesChanged;

        }
#pragma warning restore CS8618

        private void PartNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var itens = e.NewItems ?? new List<PartNode>();
            foreach (PartNode partNode in itens)
            {
                partNode.Summary.RaiseChangeEvent += Summary.RecalculateOnChange;
            }
        }

        public bool TryAdd(AddPartCmd cmd, out AddPartResult result)
        {
            if (cmd.ParentId == Id)
            {
                try
                {
                    MakeSureNameIsNotInUseInParts(cmd.Name);
                }
                catch (Exception)
                {

                    result = AddPartResult.FailureForNameAlreadyInUse;
                    return false;
                }
                var newPart = new Part(cmd.Name, cmd.ParentId);
                result = AddPartResult.Success(newPart.Id);
                AddNewPart(newPart);
                return true;
            }


            var part = _parts.Find(x => x.Id == cmd.ParentId);
            if (part != null)//when the part is found it must be turned into a PartNode
            {                //so that it will manage the new Part (from the cmd) as its child
                var newNodeFromExistingPart = part.ToPartNode();
                Add(newNodeFromExistingPart);
                RemovePartAndUpdateSummary(part);
            }

            foreach (var node in _partNodes)//when the cmd.ParentId is unknown to the current instance, delegate it to its child nodes
            {
                if (node.TryAdd(cmd, out result)) return true;
                if (result == AddPartResult.FailureForNameAlreadyInUse) return false;
            }

            result = AddPartResult.FailureForParentNodeNotFound;
            return false;
        }

        private void AddNewPart(Part newPart)
        {
            _parts.Add(newPart);
            Summary.AddOneTo(newPart.Level);
        }

        public void Add(AddPartCmd cmd)
        {
            TryAdd(cmd, out AddPartResult result);
            if (result == AddPartResult.FailureForNameAlreadyInUse) ThrowErrorForDuplicateName(cmd.Name);

        }
        
        public void ChangeLevel(string partName, Guid parentId, int newLevel)
        {
            if (parentId != Id)
            {
                foreach (var node in _partNodes)
                    node.ChangeLevel(partName, parentId, newLevel);
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
            newNode.ParentId = Id;
            MakeSureNameIsNotInUseInPartNodes(newNode.Name);
            Summary.Add(newNode.Summary);
            _partNodes.Add(newNode);
        }

        public bool TryAdd(Guid parentId, PartNode partNode)
        {
            try
            {
                if (Id == parentId)
                {
                    partNode.ParentId = Id;
                    Add(partNode);
                    return true;
                }
                foreach (var child in Nodes)
                    if (child.TryAdd(parentId, partNode))
                        return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool TryDeletePart(DeletePartCmd cmd, out DeletePartResult result)
        {
            var part = _parts.Where(x => x.Id == cmd.PartId).FirstOrDefault();
            result = DeletePartResult.NotFound404();

            if (part != null)
            {
                RemovePartAndUpdateSummary(part);
                result = DeletePartResult.NoContent204();
                return true;
            }
            else
            {
                foreach (var node in _partNodes)
                {
                    if (node.Id == cmd.PartId)
                    {
                        RemovePartNodeAndUpdateSummary(node);
                        result = DeletePartResult.NoContent204();

						return true;

                    }
                    if (node.TryDeletePart(cmd, out result))
                    {
                        if (node.IsEmpty)
                        {
                            TurnNodeIntoPart(node: node);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void RemovePartNodeAndUpdateSummary(PartNode node)
        {
            Summary.Subtract(node.Summary);
            _partNodes.Remove(node);
        }

        private void TurnNodeIntoPart(PartNode node)
        {
            RemovePartNodeAndUpdateSummary(node);
            var part = new Part(id: node.Id, level: SkillLevel.Unknown, new Name(node.Name), parent: Id);
            _parts.Add(part);
            Summary.AddOneTo(part.Level);
        }

        private void RemovePartAndUpdateSummary(Part part)
        {
            _parts.Remove(part);
            Summary.SubtractOneFrom(part.Level);
        }

        private void MakeSureNameIsNotInUseInPartNodes(string name)
        {
            foreach (var node in _partNodes)
                if (node.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    ThrowErrorForDuplicateName(name);

        }

        private void MakeSureNameIsNotInUseInParts(string name)
        {
            if (_parts.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ThrowErrorForDuplicateName(name);
        }

        private void ThrowErrorForDuplicateName(string name)
        {
            var errorMessage = string.Format(CoreMessages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, name, Name);
            throw new InvalidOperationException(errorMessage);
        }


    }
}