using AutoFixture;
using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Blueprints;
using System;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillParts
{
    public class CreationOfSkillPartShould : Test
    {

        public CreationOfSkillPartShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<PartNode>();
        }

        [Fact]
        public void HaveUniqueId()
        {
            var sut = _fixture.Create<PartNode>();
            var dummy = _fixture.Create<PartNode>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }


        [Fact]
        public void SetCreationDateToNow()
        {
            var sut = _fixture.Create<PartNode>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Create<PartNode>();

            LifecycleState actual = sut.LifecycleState;

            Assert.Equal(LifecycleState.Current, actual);
        }

    }

    
}
