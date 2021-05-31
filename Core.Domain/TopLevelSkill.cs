using Mtx.Common.Domain;
using System;

namespace Core.Domain
{
    public class TopLevelSkill : Entity
    {
        SkillModel _root;

        public TopLevelSkill(ModelName name)
        {
            _root = new SkillModel(name);
        }

        public DateTime Created => _root.Created;

        public LifecycleState LifecycleState => _root.LifecycleState;

        public string Name => _root.Name;

        public void Add(SkillModel skill)
        {
            _root.Add(skill);
        }
    }
}