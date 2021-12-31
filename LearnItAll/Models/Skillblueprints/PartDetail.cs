using System;

namespace LearnItAll.Models.Skillblueprints
{
    public class PartDetail
    {
        public Part Part { get; set; } = new Part();
        public Guid BlueprintId { get; set; }
        public virtual bool IsRoot => false;
        public virtual int IdentationLevel { get; set; } = 1;
    }

    public class RootPartDetail : PartDetail
    {
        public override bool IsRoot => true;
        public override int IdentationLevel { get; set; } = 0;
    }
}
