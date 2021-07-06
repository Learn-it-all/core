using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core
{
    public class Skill : Entity
    {
        private readonly SkillBlueprint _blueprint;

        public Skill(SkillBlueprint blueprint)
        {
            _blueprint = blueprint;
            Summary = new();
        }

        public string Name => _blueprint.Name;

        public Summary Summary { get; }
    }
}