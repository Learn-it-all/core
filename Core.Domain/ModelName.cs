using Core.Resources;
using System;

namespace Core.Domain
{
    public record ModelName
    {
        public string Value { get; init; }
        public readonly int MaxLenght = 50;

        public ModelName(string name)
        {
            if (name.Length > MaxLenght)
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(Messages.ModelName_CannotExceedMaximunLenght, MaxLenght));
            Value = name;
        }

        public static implicit operator string (ModelName name) => name.Value;
    }
}
