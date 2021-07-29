﻿using Mtx.LearnItAll.Core.Common;
using System;

namespace Mtx.LearnItAll.Core.Blueprints
{
    public record Part
    {
        public int Level { get; private set; }
         = new Unknown();
        public SkillLevel DescriptiveLevel => Level;
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ParentId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;

        public Part(Name name, Guid parent)
        {
            Name = name;
            ParentId = parent;
        }

#pragma warning disable CS8618 
        private Part() { }
#pragma warning restore CS8618 

        public void ChangeLevel(int newLevel) => Level = SkillLevel.Convert(newLevel);
        
        public static implicit operator PartNode(Part part) => new PartNode(new Name(part.Name),part.ParentId);
    }
}