using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{


    /// <summary>
    /// Represents a Skill PartNode and all the elements (other <see cref="Part"/>s and <see cref="PartNode"/>s) that might be necessary to completely describe it.
    /// </summary>
    public class PartNode
    {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; set; } = LifecycleState.Current;

        public List<PartNode> Nodes { get; set; } = new();

        public List<Part> Parts { get; set; } = new();
        public Summary Summary { get; set; } = new Summary();
        public Guid Id { get; set; } = Guid.NewGuid();

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
        public PartNode() 
        { 
        
        
        
        }
#pragma warning restore CS8618 


        public void Add(AddPartCmd cmd)
        {
            if (cmd.ParentId == Id)
            {
                MakeSureNameIsNotInUseInParts(cmd.Name);
                var newPart = new Part(cmd.Name, cmd.ParentId);
                Parts.Add(newPart);
                Summary.AddOneTo(newPart.Level);
                return;
            }
            var part = Parts.Find(x => x.Id == cmd.ParentId);
            if (part != null)
            {
                Add(part);
                Parts.Remove(part);
            }
            Nodes.ForEach(x => x.Add(cmd));

        }

        public void ChangeLevel(string partName, Guid parentId, int newLevel)
        {
            if (parentId != Id)
            {
                Nodes.ForEach(x => x.ChangeLevel(partName, parentId, newLevel));
            }
            var part = Parts.Find(x => x.Name.Equals(partName, StringComparison.OrdinalIgnoreCase));
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
            newNode.Summary.RaiseChangeEvent += Summary.RecalculateOnChange;
            Summary.Add(newNode.Summary);
            Nodes.Add(newNode);
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

        private void MakeSureNameIsNotInUseInPartNodes(string name)
        {
            if (Nodes.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ThrowErrorForDuplicateName(name);
        }

        private void MakeSureNameIsNotInUseInParts(string name)
        {
            if (Parts.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ThrowErrorForDuplicateName(name);
        }

        private void ThrowErrorForDuplicateName(string name)
        {
            var errorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, name, Name);
            throw new InvalidOperationException(errorMessage);
        }

       
    }
}