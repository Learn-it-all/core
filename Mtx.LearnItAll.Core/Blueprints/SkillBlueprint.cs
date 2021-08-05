using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
    public class SkillBlueprint : Entity
    {

#pragma warning disable CS8618
        public SkillBlueprint()
#pragma warning restore CS8618 
        {
        }

        public SkillBlueprint(Name name)
        {
            Root = new PartNode(name,Id);
            CreatedDate = DateTime.Now;
        }

        [JsonIgnore]
        public LifecycleState LifecycleState => Root.LifecycleState;
        
        public string Name => Root.Name;

        [JsonIgnore]
        public IReadOnlyCollection<PartNode> Nodes => Root.Nodes;

        [JsonIgnore]
        public IReadOnlyCollection<Part> Parts => Root.Parts;

        public PartNode Root { get ; set ; }

        public void Add(PartNode skill)
        {
            Root.Add(skill);
        }
        public void Add(AddPartCmd cmd)
        {
            Root.Add(cmd);
        }
    }
}