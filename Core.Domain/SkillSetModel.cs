using Ardalis.GuardClauses;
using Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class SkillSetModel
    {
        public IReadOnlyList<SkillModel> Skills => _skills.AsReadOnly();
        private readonly List<SkillModel> _skills = new ();
        public string Name { get; } = string.Empty;

        public Guid Id { get; private set; } = Guid.NewGuid();

        private SkillSetModel()
        {

        }

        public SkillSetModel(ModelName name) => Name = name;

        public void Add(SkillModel model)
        {
            Guard.Against.Null(model,nameof(model));
            _skills.Add(model);
        }

        public void AddNew(SkillModel skill)
        {
            if (_skills.Contains(skill))
            {
                string message = string.Format(Messages.SkillModel_ASkillWithSameNameAlreadyExistis, skill.Name, Name);
                throw new InvalidOperationException(message);
            }

            _skills.Add(skill);
        }
    }
}