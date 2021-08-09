using System;
using System.Collections.Generic;

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

        public override bool Equals(object? obj)
        {
            return obj is SummaryChangedEventArgs other &&
                   EqualityComparer<SkillLevel>.Default.Equals(Level, other.Level) &&
                   EqualityComparer<int>.Default.Equals(Difference,other.Difference);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level,Difference);
        }
    }

    
}