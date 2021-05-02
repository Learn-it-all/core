using Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class SkillSetModel
    {
        public IReadOnlyList<SkillModel> Skills => skills.AsReadOnly();
        private readonly List<SkillModel> skills = new();
        public readonly string Name = string.Empty;

        public Guid Id { get; private set; } = Guid.NewGuid();

        private SkillSetModel()
        {

        }

        public SkillSetModel(IEnumerable<SkillModel> skills, SkillSetModelName name)
        {
            if (!skills.Any())
                throw new ArgumentException(Messages.SkillSetModel_AtLeastOneSkillModelRequired,nameof(skills));

            this.skills = skills.ToList();
            Name = name;
        }
    }
}