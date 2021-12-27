using MediatR;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartCmd : IRequest<Guid>
    {

        public AddPartCmd(Name name, Guid parentId)
        {
            Name = name;
            ParentId = parentId;
        }
        public AddPartCmd(Name name, Guid parentId, Guid SkillId)
        {
            Name = name;
            ParentId = parentId;
            this.SkillId = SkillId;
        }
        public Name Name { get; }
        public Guid ParentId { get; }
        public Guid SkillId { get; }
    }
}
