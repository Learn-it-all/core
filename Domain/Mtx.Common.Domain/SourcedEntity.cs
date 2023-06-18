using System.Collections.Generic;

namespace Mtx.Common.Domain
{
	public abstract class SourcedEntity<TSource>
	{
		private readonly List<TSource> applied;
		private readonly int currentVersion;

		public List<TSource> Applied => applied;

		public int NextVersion => currentVersion + 1;

		public int CurrentVersion => currentVersion;

		protected SourcedEntity()
		{
			applied = new List<TSource>();
			currentVersion = 0;
		}

		protected SourcedEntity(IEnumerable<TSource> stream, int streamVersion)
			: this()
		{
			foreach (var source in stream)
			{
				DispatchWhen(source);
			}

			currentVersion = streamVersion;
		}

		protected void Apply(TSource source)
		{
			applied.Add(source);
			DispatchWhen(source);
		}

		protected void DispatchWhen(TSource source) => ((dynamic)this).When((dynamic)source!);
	}
}
