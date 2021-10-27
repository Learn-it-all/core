using Mtx.LearnItAll.Core.Common;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    public interface IRole
    {
        string Name { get; }
    }

    public record Role : IRole
    {
        private readonly List<SimplifiedSkill> _skills = new();
        private readonly List<Role> _specializations = new();

        public Role(Name name)
        {
            this.Name = name;
        }
        public void AddSkill(SimplifiedSkill skill)
        {
            _skills.Add(skill);
        }
        public string Name { get; }
    }
}