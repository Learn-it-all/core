using Core.Resources;
using Mtx.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class SkillModel : Entity
    {
        private List<SkillModel> _skills = new();

        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; private set; } = LifecycleState.Current;
        public IReadOnlyCollection<SkillModel> Skills { get => _skills; }

        public SkillModel(ModelName name)
        {
            Name = name;
        }

        public void Add(SkillModel skill)
        {
            MakeSureSkillNameIsNotInUse(skill.Name);
            _skills.Add(skill);
        }

        private void MakeSureSkillNameIsNotInUse(string name)
        {
            if (_skills.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                var errorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, name, Name);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public void Add(Guid parentId, SkillModel skill)
        {
            if (Id == parentId)
                Add(skill);

            var parent = _skills.SingleOrDefault(x => x.Id == parentId);
            parent?.Add(skill);
        }
    }
}