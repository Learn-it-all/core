using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Blueprints
{
    public class SkillBlueprint : Entity
    {

#pragma warning disable CS8618
        private SkillBlueprint()
#pragma warning restore CS8618 
        {
        }

        public SkillBlueprint(Name name)
        {
            _root = new PartNode(name,Id);
            CreatedDate = DateTime.Now;
        }

        [JsonIgnore]
        public LifecycleState LifecycleState => _root.LifecycleState;
        
        public string Name => _root.Name;

        [JsonIgnore]
        public IReadOnlyCollection<PartNode> Nodes => _root.Nodes;

        [JsonIgnore]
        public IReadOnlyCollection<Part> Parts => _root.Parts;

        public Summary Summary => _root.Summary.Copy();

        [JsonProperty]
        private readonly PartNode _root;

        public void Add(PartNode skill)
        {
            _root.Add(skill);
        }
        public void Add(AddPartCmd cmd)
        {
            _root.Add(cmd);
        }
    }
}