using Mtx.Common.Resources;
namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddPartResultFailure_Unknown : AddPartResult
    {
        public override int Code => 0;
        public override string Message => CommonMessages.Error;
    }
}
