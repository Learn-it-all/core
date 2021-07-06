using Mtx.Common.Domain;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
    public class SkillBlueprint : Entity
    {
        Part _root;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private SkillBlueprint()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public SkillBlueprint(Name name)
        {
            _root = new Part(name);
            CreatedDate = DateTime.Now;
        }


        public LifecycleState LifecycleState => _root.LifecycleState;

        public string Name => _root.Name;

        public IReadOnlyCollection<Part> Skills => _root.Skills;

        public void Add(Part skill)
        {
            _root.Add(skill);
        }
    }
}