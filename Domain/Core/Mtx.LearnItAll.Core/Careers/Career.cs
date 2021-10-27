using Mtx.LearnItAll.Core.Common;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    public class Career
    {
        private readonly List<Role> _roles = new();

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public IReadOnlyCollection<IRole> Roles => _roles;

        public void Add(Role role)
        {
            if (_roles.Contains(role)) return;
            _roles.Add(role);
        }

        public Career(Name name)
        {
            Name = name;
        }
    }

    public class CareerBlueprint
    {
        private readonly List<Role> _roles = new();
        private readonly List<SimplifiedSkill> _basicSkills = new();

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public IReadOnlyCollection<IRole> Roles => _roles;

        public void Add(Role role)
        {
            if (_roles.Contains(role)) return;
            _roles.Add(role);
        }

        public CareerBlueprint(Name name)
        {
            Name = name;
        }
    }

    public record SimplifiedSkill
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public SimplifiedSkill(string name, Guid id)
        {
            Name = name;
            Id = id;
        }
    }
}