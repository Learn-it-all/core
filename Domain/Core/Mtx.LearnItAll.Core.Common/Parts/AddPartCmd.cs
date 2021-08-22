using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartCmd
    {

        public AddPartCmd(Name name, Guid parentId)
        {
            Name = name;
            ParentId = parentId;
        }

        public Name Name { get; }
        public Guid ParentId { get; }
    }
}
