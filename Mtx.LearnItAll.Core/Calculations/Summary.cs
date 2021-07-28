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

        public void RecalculateOnChange(object? sender, SummaryChangedEventArgs e)
        {
           var counter = CounterFor(e.Level);
            counter.Current += e.Difference;
        }

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

        public virtual int ValueOf(SkillLevel level) => CounterFor(level).Current;
        
        public virtual Counter CounterFor(int level) => (level) switch
        {
                0 => Unfamiliar,
                1 => Novice,
                2 => AdvancedBeginner,
                3 => Competent,
                4 => Proficient,
                5 => Expert,
                _ => Unknown,
        };

        public virtual void AddOneTo(SkillLevel level) => CounterFor(level).AddOne();

        public void SubtractOneFrom(SkillLevel level) => CounterFor(level).SubtractOne();

    }
}