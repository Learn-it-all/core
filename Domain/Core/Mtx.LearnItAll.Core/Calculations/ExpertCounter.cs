namespace Mtx.LearnItAll.Core.Calculations
{
    public class ExpertCounter : Counter
    {
        public ExpertCounter() : base(SkillLevel.Expert) { }
        public ExpertCounter(int value) : base(SkillLevel.Expert, value) { }
    }
}