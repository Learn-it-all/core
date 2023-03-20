using AutoFixture;
using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common;
using System;
using Tests;
using Xunit;

namespace Skills
{

    public class CreationOfSkillShould : Test
    {

        public CreationOfSkillShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<SkillBlueprint>();
        }

        [Fact]
        public void BeAnEntity()
        {
            var sut = _fixture.Create<SkillBlueprint>();

            Assert.IsAssignableFrom<Entity>(sut);
        }

        [Fact]
        public void SetupCreationData()
        {
            var sut = _fixture.Create<SkillBlueprint>();

            DateTime actual = sut.CreatedDate;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithDraftLifecycleState()
        {
            var sut = _fixture.Create<SkillBlueprint>();

            LifecycleState actual = sut.LifecycleState;

            Assert.Equal(LifecycleState.Draft, actual);
        }

        [Fact]
        public void SetName()
        {
            var dummyName = _fixture.Create<Name>();
            var sut = _fixture.Get<Name, SkillBlueprint>((_) => new(dummyName));

            string actual = sut.Name;
            Assert.Equal(dummyName, actual);
        }

    }
}
