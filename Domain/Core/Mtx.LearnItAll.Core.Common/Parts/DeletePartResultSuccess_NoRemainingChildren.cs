namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record DeletePartResultSuccess_NoRemainingChildren : DeletePartResult
    {
        private string name;

        public DeletePartResultSuccess_NoRemainingChildren(string name)
        {
            this.name = name;
        }

        public override string Message => $"Part '{name}' successfully deleted. No child parts nor nodes exist in this instance.";
        public override int Code => 3;
        public override bool IsSuccess => true;

    }
}
