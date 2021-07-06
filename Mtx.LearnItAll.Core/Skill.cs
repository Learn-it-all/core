using Mtx.Common.Domain;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    public class Skill : Entity
    {
        SkillPart _root;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Skill()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public Skill(ModelName name)
        {
            _root = new SkillPart(name);
            CreatedDate = DateTime.Now;
        }


        public LifecycleState LifecycleState => _root.LifecycleState;

        public string Name => _root.Name;

        public IReadOnlyCollection<SkillPart> Skills => _root.Skills;

        public void Add(SkillPart skill)
        {
            _root.Add(skill);
        }
    }
}