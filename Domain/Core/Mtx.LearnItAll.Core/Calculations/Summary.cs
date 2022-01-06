using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Calculations
{
    public class Summary
    {
        public event EventHandler<SummaryChangedEventArgs>? RaiseChangeEvent;

        public virtual Counter Unknown { get; private set; } = new UnknownCounter();
        public virtual Counter Unfamiliar { get; private set; } = new UnfamiliarCounter();
        public virtual Counter Novice { get; private set; } = new NoviceCounter();
        public virtual Counter AdvancedBeginner { get; private set; } = new AdvancedBeginnerCounter();
        public virtual Counter Competent { get; private set; } = new CompetentCounter();
        public virtual Counter Proficient { get; private set; } = new ProficientCounter();
        public virtual Counter Expert { get; private set; } = new ExpertCounter();

        public void Add(Summary another)
        {
            Unknown += another.Unknown;
            Unfamiliar += another.Unfamiliar;
            Novice += another.Novice;
            AdvancedBeginner += another.AdvancedBeginner;
            Competent += another.Competent;
            Proficient += another.Proficient;
            Expert += another.Expert;
        }

        public void Subtract(Summary another)
        {
            Unknown -= another.Unknown;
            Unfamiliar -= another.Unfamiliar;
            Novice -= another.Novice;
            AdvancedBeginner -= another.AdvancedBeginner;
            Competent -= another.Competent;
            Proficient -= another.Proficient;
            Expert -= another.Expert;
        }


        public void RecalculateOnChange(object? sender, SummaryChangedEventArgs e)
        {
           var counter = CounterFor(e.Level);
            counter.Current += e.Difference;
        }


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

        public override bool Equals(object? obj)
        {
            return obj is Summary summary &&
                   EqualityComparer<Counter>.Default.Equals(Unknown, summary.Unknown) &&
                   EqualityComparer<Counter>.Default.Equals(Unfamiliar, summary.Unfamiliar) &&
                   EqualityComparer<Counter>.Default.Equals(Novice, summary.Novice) &&
                   EqualityComparer<Counter>.Default.Equals(AdvancedBeginner, summary.AdvancedBeginner) &&
                   EqualityComparer<Counter>.Default.Equals(Competent, summary.Competent) &&
                   EqualityComparer<Counter>.Default.Equals(Proficient, summary.Proficient) &&
                   EqualityComparer<Counter>.Default.Equals(Expert, summary.Expert);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Unknown, Unfamiliar, Novice, AdvancedBeginner, Competent, Proficient, Expert);
        }

        public static bool operator ==(Summary? left, Summary? right)
        {
            return EqualityComparer<Summary>.Default.Equals(left, right);
        }

        public static bool operator !=(Summary? left, Summary? right)
        {
            return !(left == right);
        }

        public Summary Copy()
        {
            return new Summary
            {
                AdvancedBeginner = AdvancedBeginner,
                Competent = Competent,
                Expert = Expert,
                Novice = Novice,
                Proficient = Proficient,
                Unfamiliar = Unfamiliar,
                Unknown = Unknown
            };
        }
    }
}