using AutoFixture;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.TopLevelSkillModels
{
    public class AddingNewSkillShould : Test
    {

        public AddingNewSkillShould()
        {

        }

        [Fact]
        public void AddAsChildOfRootSkillGivenThereIsNoNamingConflict()
        {
            var sut = _fixture.Create<Skill>();
            var dummy = _fixture.Create<SkillPart>();
            sut.Add(dummy);
            Assert.Contains(dummy, sut.Skills);
        }

        [Fact(Skip = "to be implemented")]
        public void AllowNoMoreThan2000SInnerkillModels()
        {

        }
    }
}
