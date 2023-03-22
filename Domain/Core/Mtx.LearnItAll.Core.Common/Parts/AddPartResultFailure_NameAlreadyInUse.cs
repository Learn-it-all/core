using Mtx.LearnItAll.Core.Resources;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultFailure_NameAlreadyInUse : AddPartResult
    {
        public override int Code => 2;
        public override string Message => CoreMessages.AddPartResult_DuplicateSibling;
    }
}
