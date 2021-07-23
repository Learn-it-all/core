namespace Mtx.LearnItAll.Core
{
    public record Summary
    {
        public virtual int Unfamiliar { get; private set; }
        public virtual int Novice { get; private set; }
        public virtual int AdvancedBeginner { get; private set; }
        public virtual int Competent { get; private set; }
        public virtual int Proficient { get; private set; }
        public virtual int Expert { get; private set; }
        public virtual int Unknown { get; private set; }

        public virtual void AddOneTo(SkillLevel level)
        {
            switch (level) 
            {
                case 0: Unfamiliar = ++Unfamiliar; break;
                case 1: Novice = ++Novice; break;
                case 2: AdvancedBeginner = ++AdvancedBeginner; break;
                case 3: Competent = ++Competent; break;
                case 4: Proficient = ++Proficient; break;
                case 5: Expert = ++Expert; break;
                default: ++Unknown; break;
            };
        }
    }

    public record EmptySummary : Summary
    {

    }
}