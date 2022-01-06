using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public abstract record AddPartResult
    {
        public abstract string Message { get; }
        public abstract  int Code { get; }
        public virtual bool IsSuccess => false;
        public static implicit operator int(AddPartResult result) => result.Code;
        public static AddPartResult Success(Guid idOfAddedPart) => new AddPartResultSuccess(idOfAddedPart);
        public static AddPartResult FailureForNameAlreadyInUse => new AddPartResultFailure_NameAlreadyInUse();

        public static AddPartResult FailureForUnknownReason => new AddPartResultFailure_Unknown();
        public static AddPartResult FailureForPartNotFound => new AddPartResultFailure_PartNotFound();

        public virtual Guid IdOfAddedPart => Guid.Empty;
    }
}
