using Mtx.LearnItAll.Core.Resources;
using System;

namespace Mtx.LearnItAll.Core.Common
{
    public record Name
    {
        public string Value { get; init; }
        public const int MaxLenght = 50;

        public Name(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (name.Length > MaxLenght)
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(CoreMessages.ModelName_CannotExceedMaximunLenght, MaxLenght));
            Value = name;
        }

        public static implicit operator string(Name name) => name.Value;
    }
}
