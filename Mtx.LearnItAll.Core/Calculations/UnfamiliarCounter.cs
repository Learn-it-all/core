namespace Mtx.LearnItAll.Core.Calculations
{
    public class UnfamiliarCounter : Counter
    {
        public UnfamiliarCounter() : base(SkillLevel.Unfamiliar)
        {
        }

        public UnfamiliarCounter(int value) : base(SkillLevel.Unfamiliar, value)
        {
        }

    }
}