using MediatR;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record CreateSkillBlueprintCmd : IRequest
    {
        public Name Name { get; }
        public CreateSkillBlueprintCmd(Name name)
        {
            Name = name;
        }
    }
}
