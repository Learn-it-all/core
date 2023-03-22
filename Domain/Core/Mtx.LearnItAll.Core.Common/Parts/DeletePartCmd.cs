using MediatR;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record DeletePartCmd : IRequest<DeletePartResult>
    {

        public DeletePartCmd(Guid blueprintId,  Guid partId)
        {
            BlueprintId = blueprintId;
            PartId = partId;
        }
        public Guid BlueprintId { get; }
        public Guid PartId { get; }
    }
}
