using Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public record SkillSetModelName
    {
        public string Value { get; init; }
        public readonly int MaxLenght = 50;

        public SkillSetModelName(string name)
        {
            if (name.Length > MaxLenght)
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(Messages.SkillSetModelName_CannotExceedMaximunLenght, MaxLenght));
            Value = name;
        }

        public static implicit operator string (SkillSetModelName name) => name.Value;
    }
}
