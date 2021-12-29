using MediatR;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record CreateSkillBlueprintCmd : IRequest<SkillBlueprintData>
    {
        public Name Name { get; }
        public CreateSkillBlueprintCmd(Name name)
        {
            Name = name;
        }
    }
}
