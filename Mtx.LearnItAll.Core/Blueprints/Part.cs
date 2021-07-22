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
        public void ChangeLevel(SkillLevel newLevel) => Level = newLevel;
        public void ChangeLevel(int newLevel) => Level = SkillLevel.Convert(newLevel);
    }
}