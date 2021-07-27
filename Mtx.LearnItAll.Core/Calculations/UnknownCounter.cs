namespace Mtx.LearnItAll.Core.Calculations
{
    public record UnknownCounter : Counter
    {
        public UnknownCounter() : base(SkillLevel.Unknown)
        {
        }

        public UnknownCounter(int value) : base(SkillLevel.Unknown, value)
        {
        }

    }
}