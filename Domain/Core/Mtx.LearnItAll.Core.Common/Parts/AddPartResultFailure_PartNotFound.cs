namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultFailure_PartNotFound : AddPartResult
    {
        public override int Code => 3;
        public override string Message => $"Operation failed. The provided parent is not part of the given Skill Blueprint";
    }
}
