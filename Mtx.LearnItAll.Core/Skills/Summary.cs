using System;

namespace Mtx.LearnItAll.Core
{
    public class SummaryChangedEventArgs : EventArgs
    {
        public SummaryChangedEventArgs(SkillLevel level, int difference)
        {
            Level = level;
            Difference = difference;
        }

        public SkillLevel Level { get; }
        public int Difference { get; }
    }
    public record Counter
    {

        private int _current;
        public event EventHandler<SummaryChangedEventArgs>? RaiseChangeEvent;

        public Counter(SkillLevel level)
        {
            Level = level;
            _current = 0;
        }
        public Counter(SkillLevel level, EventHandler<SummaryChangedEventArgs>? observer) :this(level)
        {
            RaiseChangeEvent = observer;
        }
        public int Current
        {
            get => _current;
            set
            {
                if (_current == value) return;

                var previous = _current;
                _current = value;
                OnLevelChanged(SkillLevel.Unknown, previous - _current);

            }
        }
        public SkillLevel Level { get; }
        private void OnLevelChanged(SkillLevel level, int difference)
        {
            RaiseChangeEvent?.Invoke(this, new(level, difference));
        }
    }

    public record Summary
    {
        public event  EventHandler<SummaryChangedEventArgs>? RaiseChangeEvent;
        private void OnCounterChanged(SummaryChangedEventArgs args)
        {
            RaiseChangeEvent?.Invoke(this, args);
        }

        public virtual Counter UnknownCounter { get; private set; } 
        public virtual int Unfamiliar { get; private set; } 
        public virtual int Novice { get; private set; } 
        public virtual int AdvancedBeginner { get; private set; } 
        public virtual int Competent { get; private set; }  
        public virtual int Proficient { get; private set; } 
        public virtual int Expert { get; private set; } 
        public virtual int Unknown { get; private set; }

        public Summary()
        {
            UnknownCounter = new(SkillLevel.Unknown, RaiseChangeEvent);
        }

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
                default: ++Unknown; UnknownCounter.Current += 1; break;
            };
        }

        internal void SubtractOneFrom(int level)
        {
            switch (level)
            {
                case 0: Unfamiliar = --Unfamiliar; break;
                case 1: Novice = --Novice; break;
                case 2: AdvancedBeginner = --AdvancedBeginner; break;
                case 3: Competent = --Competent; break;
                case 4: Proficient = --Proficient; break;
                case 5: Expert = --Expert; break;
                default: --Unknown; break;
            };
        }
        
    }
}