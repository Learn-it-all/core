using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LearnItAll.Models.Skillblueprints
{
    public class SkillBluePrint
    {
        public Guid Id {get;set;}

        public virtual bool IsNull => false;

        public string Name => _root.Name;

        [JsonIgnore]
        public IReadOnlyCollection<PartNode> Nodes => _root.Nodes;

        [JsonIgnore]
        public IReadOnlyCollection<Part> Parts => _root.Parts;

        [JsonIgnore]
        public Guid SkillId => _root.Id;

        [JsonProperty]
        private readonly PartNode _root = new NullPartNode();

#pragma warning disable CS8618
        public SkillBluePrint()
        {

        }
#pragma warning restore CS8618

    }

    public class NullSkillBluePrint : SkillBluePrint
    {
        public override bool IsNull => true;
        internal static SkillBluePrint New()
        {
            return new NullSkillBluePrint();
        }

    }
    public class NullPartNode : PartNode
    {
        public override bool IsNull => true;
    }
}
