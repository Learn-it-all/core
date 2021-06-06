using AutoFixture;
using Core.Domain;
using Mtx.Common.Domain;
using System;
using Xunit;

namespace Core.Tests.SkillModels
{
    public class CreationOfSkillModelShould : Test
    {

        public CreationOfSkillModelShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<SkillModel>();
        }

        [Fact]
        public void HaveUniqueId()
        {
            var sut = _fixture.Create<SkillModel>();
            var dummy = _fixture.Create<SkillModel>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }

        [Fact]
        public void BeAnEntity()
        {
            var sut = _fixture.Create<SkillModel>();

            Assert.IsAssignableFrom<Entity>(sut);
        }

        [Fact]
        public void SetCreationDateToNow()
        {
            var sut = _fixture.Create<SkillModel>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Create<SkillModel>();

            LifecycleState actual = sut.LifecycleState;

            Assert.Equal(LifecycleState.Current, actual);
        }

    }

    
}
