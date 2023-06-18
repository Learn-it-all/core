using System;

namespace Mtx.Common.Domain
{
	public interface ISourceType
	{
		public DateTimeOffset OccurredOn { get; }

		public DateTimeOffset ValidOn { get; }
	}
}
