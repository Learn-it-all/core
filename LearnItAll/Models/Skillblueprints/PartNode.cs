using System;
using System.Collections.Generic;

namespace LearnItAll.Models.Skillblueprints
{
    public class PartNode
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Part> Parts { get; set; } = new();
        public List<PartNode> Nodes { get; set; } = new();
        public virtual bool IsNull => false;
    }
}
