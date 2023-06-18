using Mtx.LearnItAll.Core.Resources;

namespace Mtx.LearnItAll.Core.Common
{
	public record Name
	{
		public string Value { get; init; }
		public const int MaxLength = 50;

		public Name(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
			}

			if (name.Length > MaxLength)
				throw new ArgumentOutOfRangeException(nameof(name), string.Format(CoreMessages.ModelName_CannotExceedMaximunLenght, MaxLength));
			Value = name;
		}
		public static Name From(string name) => new Name(name);
		public static implicit operator string(Name name) => name.Value;
	}
	public static class NameExtensions
	{
		public static Name ToName(this string me) => new Name(me);
	}
}
