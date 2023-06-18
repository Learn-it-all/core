using System;

namespace Mtx.Common.Domain
{
	public record UniqueId
	{
		private UniqueId()
		{
			Value = Guid.NewGuid().ToString();
		}

		private UniqueId(string provided)
		{
			if (string.IsNullOrEmpty(provided))
			{
				throw new ArgumentException($"'{nameof(provided)}' cannot be null or empty.", nameof(provided));
			}
			Value = provided;
		}

		public string Value { get; }
		public static UniqueId New() => new UniqueId();
		public static UniqueId From(string provided) => new UniqueId(provided);
		public static implicit operator string(UniqueId id) => id.Value;
	}
}
