using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public interface ISkillBlueprintGrain : IGrainWithStringKey
    {
        Guid SkillId { get; }

        Task Add(PartNode part);
        Task Create(Name skillName);
        //Task AddPart(AddPartCmd newPart);
    }

    public class SkillBlueprintGrain : Grain, ISkillBlueprintGrain
    {
        private static string GrainType => nameof(SkillBlueprintGrain);
        private Guid GrainKey => this.GetPrimaryKey();
        public Guid SkillId => _skill?.Id ?? Guid.Empty;
        private readonly IPersistentState<SkillBlueprintState> _state;
        private  SkillBlueprint? _skill => _state.State.Skill;
        public SkillBlueprintGrain([PersistentState("skillBlueprint", storageName:"skillBlueprintState")] IPersistentState<SkillBlueprintState> state)
        {
            _state = state;
        }

        public async Task Create(Name skillName)
        {
            _state.State.Skill = new SkillBlueprint(skillName);
            await _state.WriteStateAsync();
        }

        public async Task Add(PartNode part)
        {
            _skill?.Add(part);
            await _state.WriteStateAsync();


        }

        public override Task OnActivateAsync()
        {
            // initialize state as needed
            if (_state.State.Skill == null) _state.State.Skill = new SkillBlueprint(new Name(base.GrainReference.GetPrimaryKeyString()));

            return Task.CompletedTask;
        }

        //public async Task AddPart(AddPartCmd newPart)
        //{
        //    _skill?.Add(newPart);
        //    await _state.WriteStateAsync();
        //}

        public class SkillBlueprintState
        {
            public SkillBlueprint? Skill { get; set; }
        }
    }
}
