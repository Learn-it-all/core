using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record SkillBlueprintData(Guid Id, Guid RootPartId)
	{
        public static SkillBlueprintData New(Guid id, Guid rootPartId) => new SkillBlueprintData(id, rootPartId);
    }
}
