namespace Mtx.LearnItAll.Core.Calculations
{
    public class ProficientCounter : Counter
    {
        public ProficientCounter() : base(SkillLevel.Proficient)
        {
        }

        public ProficientCounter(int value) : base(SkillLevel.Proficient, value)
        {
        }

    }
}