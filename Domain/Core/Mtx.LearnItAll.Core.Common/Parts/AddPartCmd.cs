using MediatR;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartCmd : IRequest<AddPartResult>
    {

        public AddPartCmd(Name name, Guid parentId)
        {
            Name = name;
            ParentId = parentId;
        }
        public AddPartCmd(Name name, Guid parentId, Guid blueprintId)
        {
            Name = name;
            ParentId = parentId;
            this.BlueprintId = blueprintId;
        }
        public Name Name { get; }
        public Guid ParentId { get; }
        public Guid BlueprintId { get; }
    }
}
