using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Tests;
using Xunit;

namespace Blueprints
{
    public class CreatingNewSkillBlueprintShould : Test
    {

        [Fact]
        public void AddAsChildOfRootSkillGivenThereIsNoNamingConflict()
        {
            var sut = _fixture.Create<SkillBlueprint>();
            var dummy = _fixture.Create<PartNode>();
            sut.Add(dummy);
            Assert.Contains(dummy, sut.Nodes);
        }

        [Fact(Skip = "to be implemented")]
        public void AllowNoMoreThan2000SInnerkillModels()
        {

        }
    }
}