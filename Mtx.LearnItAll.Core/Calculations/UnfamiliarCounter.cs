namespace Mtx.LearnItAll.Core.Calculations
{
    public record UnfamiliarCounter : Counter
    {
        public UnfamiliarCounter() : base(SkillLevel.Unfamiliar)
        {
        }

        public UnfamiliarCounter(int value) : base(SkillLevel.Unfamiliar, value)
        {
        }

    }
}