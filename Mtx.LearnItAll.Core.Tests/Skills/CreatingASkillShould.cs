using Xunit;
using AutoFixture;
using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core.Tests.Skills
{
    public class CreatingASkillShould : Test
    {
        public CreatingASkillShould()
        {
        }

        [Fact]
        public void BeAnEntity()
        {
            Skill sut = _fixture.Create<Skill>();
            Assert.IsAssignableFrom<Entity>(sut);
        }

        [Fact]
        public void AcceptTheSkillBlueprintGivenItsValid()
        {
            var dummy = _fixture.Create<SkillBlueprint>();
            
            var sut = new Skill(blueprint:dummy);

            Assert.Equal(dummy.Name, sut.Name);
        }

        [Fact]
        public void SetDefaultSkillLevelGivenASinglePartSkillBlueprint()
        {
            var expected = _fixture.Create<Summary>();
            var sut = _fixture.Create<Skill>();

            Assert.Equal(expected, (Summary)sut.Summary);

        }

        [Fact]
        public void SetDefaultSkillScaleGivenAMultiPartSkillBlueprint()
        {
        }
        [Fact]
        public void RaiseSkillLevelToNoviceOnSkillAndAllPartsGivenSkillLevelIsUnfamiliar()
        {
        }

        [Fact]
        public void CalculateSkillLevelGivenSkillHasParts()
        {
        }

        [Fact]
        public void CalculateSkillLevelGivenSkillBlueprintHasNoParts()
        {
        }

    }

}
