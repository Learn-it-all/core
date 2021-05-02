using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Core.Domain
{
    public class SkillSet
    {
        public IReadOnlyList<Skill> Skills => skills.AsReadOnly();
        private readonly List<Skill> skills = new();

        public SkillSet(Skill skill)
        {
            if (skill is null)
            {
                throw new System.ArgumentNullException(nameof(skill));
            }

            skills.Add(skill);
        }

        private SkillSet() { }

        public SkillSet(IEnumerable<Skill> skills)
        {
            if (skills is null)
            {
                throw new System.ArgumentNullException(nameof(skills));
            }

            this.skills = skills.ToList();
        }
    }
}