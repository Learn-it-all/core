namespace Mtx.LearnItAll.Core.Calculations
{
    public record ExpertCounter : Counter
    {
        public ExpertCounter() : base(SkillLevel.Expert)
        {
        }

        public ExpertCounter(int value) : base(SkillLevel.Expert, value)
        {
        }

    }
}