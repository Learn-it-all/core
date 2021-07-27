using System;

namespace Mtx.LearnItAll.Core.Calculations
{
    public record Counter
    {

        private int _current = 0;
        public event EventHandler<SummaryChangedEventArgs>? RaiseChangeEvent;

        public Counter(SkillLevel level)
        {
            Level = level;
        }
        public Counter(SkillLevel level, int value) : this(level)
        {
            _current = value;
        }
        public int Current
        {
            get => _current;
            set
            {
                if (_current == value) return;

                var previous = _current;
                _current = value;
                OnLevelChanged(Level, previous + _current);

            }
        }
        public SkillLevel Level { get; }

        public void AddOne()
        {
            Current = ++Current;
        }
        public void SubtractOne()
        {
            Current = --Current;

        }

        protected virtual void OnLevelChanged(SkillLevel level, int difference)
        {
            RaiseChangeEvent?.Invoke(this, new(level, difference));
        }

        public static Counter operator +(Counter right, int left)
        {
            right.Current += left;
            return right;
        }
        public static Counter operator +(Counter right, Counter left)
        {
            right.Current += left.Current;
            return right;
        }

        public static Counter operator -(Counter right, int left)
        {
            right.Current -= left;
            return right;
        }
        public static Counter operator -(Counter right, Counter left)
        {
            right.Current -= left.Current;
            return right;
        }

        public static implicit operator int(Counter counter) => counter.Current;


    }
}