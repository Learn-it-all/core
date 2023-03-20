using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddMultiplePartsCmd : IRequest<AddMultiplePartsResult>
    {
        private readonly List<Name> _names;

        public AddMultiplePartsCmd(IEnumerable<Name> names, Guid parentId, Guid blueprintId)
        {
            _names = names?.ToList() ?? throw new ArgumentNullException(nameof(names));
            ParentId = parentId;
            BlueprintId = blueprintId;
        }
        public IReadOnlyList<Name> Names => _names;
        public Guid ParentId { get; }
        public Guid BlueprintId { get; }
    }
}
