using System;

namespace LearnItAll.Models.Skillblueprints
{
    public class PartNodeDetail
    {
        public PartNode PartNode { get; set; } = new NullPartNode();
        public Guid SkillId { get; set; }

    }
}
