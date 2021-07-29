using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Calculations
{
    public class Counter
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

        private Counter() { }

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
        public int Level { get; }

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

        public static Counter Create(SkillLevel skillLevel) => Create(skillLevel);
        
        public static Counter Create(int skillLevel) => (skillLevel) switch
        {
            0 => new UnfamiliarCounter(),
            1 => new NoviceCounter(),
            2 => new AdvancedBeginnerCounter(),
            3 => new CompetentCounter(),
            4 => new ProficientCounter(),
            5 => new ExpertCounter(),
            _ => new UnknownCounter(),
        };
        public override bool Equals(object? obj)
        {
            return obj is Counter counter &&
                   _current == counter._current &&
                   Current == counter.Current &&
                   Level == counter.Level;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_current, Current, Level);
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

        public static bool operator ==(Counter? left, Counter? right)
        {
            return EqualityComparer<Counter>.Default.Equals(left, right);
        }

        public static bool operator !=(Counter? left, Counter? right)
        {
            return !(left == right);
        }
    }
}