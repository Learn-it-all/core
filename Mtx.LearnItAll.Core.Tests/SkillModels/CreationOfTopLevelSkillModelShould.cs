using AutoFixture;
using Mtx.LearnItAll.Core;
using Mtx.Common.Domain;
using System;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillModels
{

    public class CreationOfTopLevelSkillModelShould : Test
    {

        public CreationOfTopLevelSkillModelShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<TopLevelSkill>();
        }

        [Fact]
        public void BeAnEntity()
        {
            var sut = _fixture.Create<TopLevelSkill>();

            Assert.IsAssignableFrom<Entity>(sut);
        }

        [Fact]
        public void SetupCreationData()
        {
            var sut = _fixture.Create<TopLevelSkill>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Create<TopLevelSkill>();

            LifecycleState actual = sut.LifecycleState;
            Assert.Equal(LifecycleState.Current, actual);
        }

        [Fact]
        public void SetName()
        {
            var dummyName = _fixture.Create<ModelName>();
            var sut = _fixture.Get<ModelName,TopLevelSkill>((_)=> new(dummyName));

            string actual = sut.Name;
            Assert.Equal(dummyName, actual);



        }

    }

    
}
