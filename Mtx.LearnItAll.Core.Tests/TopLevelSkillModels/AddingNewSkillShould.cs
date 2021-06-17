using AutoFixture;
using System.Collections.Generic;
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
            var sut = _fixture.Create<TopLevelSkill>();
            var dummy = _fixture.Create<SkillModel>();
            sut.Add(dummy);
            Assert.Contains(dummy, sut.Skills);
        }
    }
}
