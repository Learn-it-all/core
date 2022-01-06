namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultFailure_NameAlreadyInUse : AddPartResult
    {
        public override int Code => 2;
        public override string Message => $"Operation failed. The provided name is already a child of the given parent.";
    }
}
