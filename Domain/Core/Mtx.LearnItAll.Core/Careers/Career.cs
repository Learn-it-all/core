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
        public IReadOnlyCollection<Role> Roles => _roles;

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
}