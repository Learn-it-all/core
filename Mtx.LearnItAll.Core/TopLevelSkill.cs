using Mtx.Common.Domain;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    public class TopLevelSkill : Entity
    {
        SkillModel _root;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private TopLevelSkill()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public TopLevelSkill(ModelName name)
        {
            _root = new SkillModel(name);
            CreatedDate = DateTime.Now;
        }


        public LifecycleState LifecycleState => _root.LifecycleState;

        public string Name => _root.Name;

        public IReadOnlyCollection<SkillModel> Skills => _root.Skills;

        public void Add(SkillModel skill)
        {
            _root.Add(skill);
        }
    }
}