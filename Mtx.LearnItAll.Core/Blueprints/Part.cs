using Mtx.LearnItAll.Core.Common;
using System;

namespace Mtx.LearnItAll.Core.Blueprints
{
    public record Part
    {
        public int Level { get;  set; }
         = new Unknown();
        public Guid Id { get;  set; } = Guid.NewGuid();
        public Guid ParentId { get;  set; }
        public string Name { get;  set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public Part(Name name, Guid parent)
        {
            Name = name;
            ParentId = parent;
        }

#pragma warning disable CS8618 
        public Part() { }
#pragma warning restore CS8618 

        public PartNode ToPartNode()
        {
            return this;
        }

        public void ChangeLevel(int newLevel) => Level = SkillLevel.Convert(newLevel);
        
        public static implicit operator PartNode(Part part) => new PartNode(new Name(part.Name),part.ParentId);
    }
}