using System;

namespace LearnItAll.Models.Skillblueprints
{
    public class PartDetail
    {
        public Part Part { get; set; } = new Part();
        public Guid BlueprintId { get; set; }
        public virtual bool IsRoot => false;
        public virtual int IdentationLevel { get; set; } = 1;
        public virtual string PartialName(int length = 10, string suffix = "...")
        {
            var nameLength = Part.Name.Length;
            var suffixLength = suffix.Length;
            var totalLength = length + suffixLength;
            if (nameLength <= totalLength) return Part.Name;
            return Part.Name.Substring(0,length) + suffix;

        }
    }

    public class RootPartDetail : PartDetail
    {
        public override bool IsRoot => true;
        public override int IdentationLevel { get; set; } = 0;
    }
}
