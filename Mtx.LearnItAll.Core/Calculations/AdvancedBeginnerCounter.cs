﻿namespace Mtx.LearnItAll.Core.Calculations
{
    public record AdvancedBeginnerCounter : Counter
    {
        public AdvancedBeginnerCounter() : base(SkillLevel.AdvancedBeginner)
        {
        }

        public AdvancedBeginnerCounter(int value) : base(SkillLevel.AdvancedBeginner, value)
        {
        }

    }
}