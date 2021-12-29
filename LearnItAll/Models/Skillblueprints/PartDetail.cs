using System;

namespace LearnItAll.Models.Skillblueprints
{
    public class PartDetail
    {
        public Part Part { get; set; } = new Part();
        public Guid BlueprintId { get; set; }
        public virtual bool IsRoot => false;
    }

    public class RootPartDetail : PartDetail
    {
        public override bool IsRoot => true;
    }
}
