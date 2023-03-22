namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record DeletePartResultSuccess : DeletePartResult
    {
        private string name;

        public DeletePartResultSuccess(string name)
        {
            this.name = name;
        }

        public override string Message => $"Part '{name}' successfully deleted";
        public override int Code => 1;
        public override bool IsSuccess => true;

    }
}
