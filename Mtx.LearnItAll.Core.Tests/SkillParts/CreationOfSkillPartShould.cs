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
            _ = _fixture.Create<Part>();
        }

        [Fact]
        public void HaveUniqueId()
        {
            var sut = _fixture.Create<Part>();
            var dummy = _fixture.Create<Part>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }


        [Fact]
        public void SetCreationDateToNow()
        {
            var sut = _fixture.Create<Part>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void StartWithCurrentLifecycleState()
        {
            var sut = _fixture.Create<Part>();

            LifecycleState actual = sut.LifecycleState;

            Assert.Equal(LifecycleState.Current, actual);
        }

    }

    
}
