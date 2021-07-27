using System;

namespace Mtx.LearnItAll.Core.Calculations
{
    public class SummaryChangedEventArgs : EventArgs
    {
        public SummaryChangedEventArgs(SkillLevel level, int difference)
        {
            Level = level;
            Difference = difference;
        }

        public int Level { get; }
        public int Difference { get; }
    }
}