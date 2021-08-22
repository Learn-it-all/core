using Mtx.LearnItAll.Core.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Mtx.LearnItAll.Core.Common
{
    [TypeConverter(typeof(NameConverter))]
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

    public class NameConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string s)
                return new Name(s);
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if(destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Name name)
            {
                return name.Value;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
