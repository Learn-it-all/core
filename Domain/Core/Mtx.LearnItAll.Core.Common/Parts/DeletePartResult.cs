using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public abstract record DeletePartResult
    {
        public abstract string Message { get; }
        public abstract int Code { get; }
        public virtual bool IsSuccess => false;
        public static implicit operator int(DeletePartResult result) => result.Code;
        public static DeletePartResult Success(string name) => new DeletePartResultSuccess(name);

        public static DeletePartResult FailureForPartNotFound => new DeletePartResultFailure_PartNotFound();
    }
}
