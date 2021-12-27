using System;

namespace LearnItAll.Models.Skillblueprints
{
    public class Part
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
