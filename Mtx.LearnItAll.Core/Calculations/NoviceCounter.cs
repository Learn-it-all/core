namespace Mtx.LearnItAll.Core.Calculations
{
    public record NoviceCounter : Counter
    {
        public NoviceCounter() : base(SkillLevel.Novice)
        {
        }

        public NoviceCounter(int value) : base(SkillLevel.Novice, value)
        {
        }

    }
}