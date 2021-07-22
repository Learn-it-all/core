using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core
{
    public class SkillSetModel
    {
        public IReadOnlyList<PartNode> Skills => _skills.AsReadOnly();
        private readonly List<PartNode> _skills = new ();
        public  readonly int MaximumDirectSkillModelChild = 50;

        public string Name { get; } = string.Empty;

        public Guid Id { get; private set; } = Guid.NewGuid();

        private SkillSetModel()
        {

        }

        public SkillSetModel(Name name) => Name = name;

        public void AddNew(PartNode skill)
        {
            MakeSureSkillModelIsNotAlreadyInUse(skill);
            MakeSureMaximumNumberOfDirectSkillModelChildrenIsNotExceeded();
            _skills.Add(skill);
        }

        private void MakeSureMaximumNumberOfDirectSkillModelChildrenIsNotExceeded()
        {
            if (_skills.Count == MaximumDirectSkillModelChild)
            {
                string message = BuildErrorMessageForMaximumNumberOfSkillModels();
                throw new InvalidOperationException(message);
            }
        }

        private void MakeSureSkillModelIsNotAlreadyInUse(PartNode skill)
        {
            if (_skills.Contains(skill))
            {
                string message = BuildErrorMessageForExistingSkillModel(skill);
                throw new InvalidOperationException(message);
            }
        }

        private string BuildErrorMessageForMaximumNumberOfSkillModels()
        {
            return string.Format(Messages.SkillModelSet_MaximumDirectSkillModelChildExceeded, MaximumDirectSkillModelChild);
        }

        private string BuildErrorMessageForExistingSkillModel(PartNode skill)
        {
            return string.Format(Messages.SkillModel_ASkillWithSameNameAlreadyExistis, skill.Name, Name);
        }
    }
}