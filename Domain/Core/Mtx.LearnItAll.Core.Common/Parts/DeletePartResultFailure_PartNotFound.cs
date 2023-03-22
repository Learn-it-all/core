namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record DeletePartResultFailure_PartNotFound : DeletePartResult
    {
        public override string Message => $"Operation failed. Part not found";
        public override int Code => 2;
    }
}
