using AutoFixture;
using Mtx.Common.Domain;
using System;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillSetModels
{

    public class CreationOfTopLevelSkillModelShould : Test
    {

        public CreationOfTopLevelSkillModelShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<Skill>();
        }

        [Fact]
        public void BeAnEntity()
        {
            var sut = _fixture.Create<Skill>();

            Assert.IsAssignableFrom<Entity>(sut);
        }

        [Fact]
        public void SetupCreationData()
        {
            var sut = _fixture.Build<Skill>().OmitAutoProperties().Create();

            DateTime actual = sut.CreatedDate;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Build<Skill>().OmitAutoProperties().Create();

            LifecycleState actual = sut.LifecycleState;
            
            Assert.Equal(LifecycleState.Current, actual);
        }

        [Fact]
        public void SetName()
        {
            var dummyName = _fixture.Create<ModelName>();
            var sut = _fixture.Get<ModelName,Skill>((_)=> new(dummyName));

            string actual = sut.Name;
            Assert.Equal(dummyName, actual);
        }

    }

    
}
