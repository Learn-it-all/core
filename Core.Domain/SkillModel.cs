using Mtx.Common.Domain;
using System;

namespace Core.Domain
{
    public class SkillModel : Entity
    {
        public string Name { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();

        public SkillModel(ModelName name)
        {
            Name = name;
        }
    }
}