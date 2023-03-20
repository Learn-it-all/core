using Mtx.LearnItAll.Core.Resources;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultFailure_PartNotFound : AddPartResult
    {
        public override int Code => 3;
        public override string Message => CoreMessages.Skill_ParentIdDoesNotExist;
    }
}
