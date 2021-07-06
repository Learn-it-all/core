using Mtx.LearnItAll.Core.Resources;
using Mtx.Common.Domain;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    /// <summary>
    /// Represents a Skill and all the elements (other <see cref="SkillPart"/>s) that might be necessary to completely describe it.
    /// </summary>
    public class SkillPart
    {
        private readonly List<SkillPart> _skills = new();

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public LifecycleState LifecycleState { get; private set; } = LifecycleState.Current;
        public IReadOnlyCollection<SkillPart> Skills { get => _skills; }

        public SkillPart(ModelName name)
        {
            Name = name;
        }

        /// <summary>
        /// For EF Core
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private SkillPart() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Add a <see cref="SkillPart"/> as a direct child.
        /// </summary>
        /// <param name="skill">the Skill to be added a direct child</param>
        /// <exception cref="InvalidOperationException">When the name of the <paramref name="skill"/> already exists as a direct child of current object.</exception>
        public void Add(SkillPart skill)
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

        /// <summary>
        /// Adds the <paramref name="skill"/> to any node in the graph that matches the <paramref name="parentId"/>
        /// </summary>
        /// <param name="parentId">The parent under which the <paramref name="skill"/> will be added to</param>
        /// <param name="skill">The <see cref="SkillPart"/> to be added</param>
        /// <returns>true when the <paramref name="parentId"/> was found and the <paramref name="skill"/> added as its child. False when no parent with <paramref name="parentId"/> was found.</returns>
        /// <exception cref="InvalidOperationException">When the name of the <paramref name="skill"/>already exists as a direct child of <paramref name="parentId"/></exception>
        public bool TryAdd(Guid parentId, SkillPart skill)
        {
            if (Id == parentId)
            {
                Add(skill);
                return true;
            }
            foreach (var child in _skills)
                if (child.TryAdd(parentId, skill))
                    return true;
            return false;
        }
    }
}