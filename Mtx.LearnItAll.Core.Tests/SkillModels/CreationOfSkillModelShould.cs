using AutoFixture;
using Mtx.Common.Domain;
using System;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillModels
{
    public class CreationOfSkillModelShould : Test
    {

        public CreationOfSkillModelShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<SkillPart>();
        }

        [Fact]
        public void HaveUniqueId()
        {
            var sut = _fixture.Create<SkillPart>();
            var dummy = _fixture.Create<SkillPart>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }


        [Fact]
        public void SetCreationDateToNow()
        {
            var sut = _fixture.Create<SkillPart>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Create<SkillPart>();

            LifecycleState actual = sut.LifecycleState;

            Assert.Equal(LifecycleState.Current, actual);
        }

    }

    
}
