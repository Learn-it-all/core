using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record SkillBlueprintData
    {
        public Guid Id { get; set; }
        public Guid RootPartId { get; set; }
    }
}
