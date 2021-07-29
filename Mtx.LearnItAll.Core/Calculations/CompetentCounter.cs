﻿namespace Mtx.LearnItAll.Core.Calculations
{
    public class CompetentCounter : Counter
    {
        public CompetentCounter() : base(SkillLevel.Competent)
        {
        }

        public CompetentCounter(int value) : base(SkillLevel.Competent, value)
        {
        }

    }
}