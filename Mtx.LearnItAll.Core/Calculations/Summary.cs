using System;

namespace Mtx.LearnItAll.Core.Calculations
{
    public record Summary
    {
        public event EventHandler<SummaryChangedEventArgs>? RaiseChangeEvent;

        public virtual Counter Unknown { get; private set; } = new UnknownCounter();
        public virtual Counter Unfamiliar { get; private set; } = new UnfamiliarCounter();
        public virtual Counter Novice { get; private set; } = new NoviceCounter();
        public virtual Counter AdvancedBeginner { get; private set; } = new AdvancedBeginnerCounter();
        public virtual Counter Competent { get; private set; } = new CompetentCounter();
        public virtual Counter Proficient { get; private set; } = new ProficientCounter();
        public virtual Counter Expert { get; private set; } = new ExpertCounter();

        public Summary()
        {
            Unknown.RaiseChangeEvent += Counter_RaiseChangeEvent;
            Unfamiliar.RaiseChangeEvent += Counter_RaiseChangeEvent;
            Novice.RaiseChangeEvent += Counter_RaiseChangeEvent;
            AdvancedBeginner.RaiseChangeEvent += Counter_RaiseChangeEvent;
            Competent.RaiseChangeEvent += Counter_RaiseChangeEvent;
            Proficient.RaiseChangeEvent += Counter_RaiseChangeEvent;
            Expert.RaiseChangeEvent += Counter_RaiseChangeEvent;
        }

        private void Counter_RaiseChangeEvent(object? sender, SummaryChangedEventArgs e)
        {
            RaiseChangeEvent?.Invoke(this, e);

        }

        public virtual void AddOneTo(SkillLevel level)
        {
            switch (level)
            {
                case 0: Unfamiliar.AddOne(); break;
                case 1: Novice.AddOne(); break;
                case 2: AdvancedBeginner.AddOne(); break;
                case 3: Competent.AddOne(); break;
                case 4: Proficient.AddOne(); break;
                case 5: Expert.AddOne(); break;
                default: Unknown.AddOne(); break;
            };
        }

        public void SubtractOneFrom(SkillLevel level)
        {
            switch (level)
            {
                case 0: Unfamiliar.SubtractOne(); break;
                case 1: Novice.SubtractOne(); break;
                case 2: AdvancedBeginner.SubtractOne(); break;
                case 3: Competent.SubtractOne(); break;
                case 4: Proficient.SubtractOne(); break;
                case 5: Expert.SubtractOne(); break;
                default: Unknown.SubtractOne(); break;
            };
        }

    }
}