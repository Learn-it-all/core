namespace Mtx.LearnItAll.Core.Calculations
{
    public class NoviceCounter : Counter
    {
        public NoviceCounter() : base(SkillLevel.Novice) { }
        public NoviceCounter(int value) : base(SkillLevel.Novice, value) { }
    }
}