using MediatR;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddManyPartsCmd : IRequest<AddPartResult>
    {

        public AddManyPartsCmd(IEnumerable<Name> names, Guid parentId)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            ParentId = parentId;
        }
        public AddManyPartsCmd(IEnumerable<Name> names, Guid parentId, Guid blueprintId) : this(names, parentId)
        {
            this.BlueprintId = blueprintId;
        }
        public IEnumerable<Name> Names { get; }

        public Guid ParentId { get; }
        public Guid BlueprintId { get; }
    }
}
