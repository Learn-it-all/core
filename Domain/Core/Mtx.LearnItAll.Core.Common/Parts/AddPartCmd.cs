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



    public record AddPartResultFailure_Unknown : AddPartResult
    {
        public override int Code => 0;
        public override string Message => "Operation failed for unknown reason.";
    }

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

    public record AddPartResultFailure_NameAlreadyInUse : AddPartResult
    {
        public override int Code => 2;
        public override string Message => $"Operation failed. The provided name is already a child of the given parent.";
    }

    public record AddPartResultFailure_PartNotFound : AddPartResult
    {
        public override int Code => 3;
        public override string Message => $"Operation failed. The provided parent is not part of the given Skill Blueprint";
    }
}
