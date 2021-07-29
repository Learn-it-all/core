using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.TopLevelSkillModels
{
    public class AddingNewSkillBlueprintShould : Test
    {

        public AddingNewSkillBlueprintShould()
        {

        }

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
