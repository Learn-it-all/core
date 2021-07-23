using Mtx.LearnItAll.Core.Resources;
using System;

namespace Mtx.LearnItAll.Core.Common
{
    public record Name
    {
        public string Value { get; init; }
        public readonly int MaxLenght = 50;

        public Name(string name)
        {
            if (name.Length > MaxLenght)
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(Messages.ModelName_CannotExceedMaximunLenght, MaxLenght));
            Value = name;
        }

        public static implicit operator string(Name name) => name.Value;
    }
}
