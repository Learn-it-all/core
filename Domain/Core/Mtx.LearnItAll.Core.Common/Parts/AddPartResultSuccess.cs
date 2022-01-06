using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultSuccess : AddPartResult
    {
        public AddPartResultSuccess(Guid idOfAddedPart)
        {
            IdOfAddedPart = idOfAddedPart;
        }
        public override Guid IdOfAddedPart { get; }
        public override bool IsSuccess => true;

        public override int Code => 1;
        public override string Message => "Operation succeeded.";
    }
}
