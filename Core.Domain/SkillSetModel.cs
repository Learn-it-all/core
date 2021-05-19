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
        private readonly List<SkillModel> _skills = new();
        public string Name { get; } = string.Empty;

        public Guid Id { get; private set; } = Guid.NewGuid();

        private SkillSetModel()
        {

        }

        public SkillSetModel(SkillSetModelName name) => Name = name;

        public void Add(SkillModel model)
        {
            Guard.Against.Null(model,nameof(model));
            _skills.Add(model);
        }
    }
}