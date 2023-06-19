using Mtx.Common.Domain;
using System;

namespace Mtx.LearnItAll.Core.Blueprints
{
	public abstract record SkillBluePrintEvent : DomainEvent
	{
		protected SkillBluePrintEvent()
		{
		}

		protected SkillBluePrintEvent(DomainEvent original) : base(original)
		{
		}

		protected SkillBluePrintEvent(int version) : base(version)
		{
		}

		protected SkillBluePrintEvent(DateTimeOffset validOn, int version) : base(validOn, version)
		{
		}
	}
}