using MediatR;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record DeleteBlueprintCmd : IRequest<DeleteBlueprintResult>
    {

        public DeleteBlueprintCmd(Guid blueprintId)
        {
            BlueprintId = blueprintId;
        }
        public Guid BlueprintId { get; }
    }
}
