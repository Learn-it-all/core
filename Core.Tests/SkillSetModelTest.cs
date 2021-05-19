using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Core.Domain;
using Xunit;

namespace Core.Tests
{
    public class SkillSetModelTest

    {
        private Fixture _fixture => new Fixture();

        public SkillSetModelTest()
        {

        }

        [Fact]
        public void Exists()
        {
            _ = _fixture.Create<SkillSetModel>();

        }

        [Fact]
        public void SkillsAreNeverNull()
        {
            var sut = _fixture.Create<SkillSetModel>();

            Assert.NotNull(sut.Skills);
        }


        

        [Fact]
        public void HasUniqueId()
        {
            var sut = _fixture.Create<SkillSetModel>();
            var anotherSut = _fixture.Create<SkillSetModel>();

            Assert.NotEqual(Guid.Empty, sut.Id);
            Assert.NotEqual(anotherSut.Id, sut.Id);
        }

        [Fact]
        public void HasName()
        {
            var expected = _fixture.Create<SkillSetModelName>();
            var sut = _fixture.Get((SkillSetModelName _) => new SkillSetModel(expected));

            string actual = sut.Name;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddSkill()
        {
            var sut = _fixture.Create<SkillSetModel>();
            var dummySkill = _fixture.Create<SkillModel>();

            sut.Add(model: dummySkill);

            Assert.Contains(dummySkill, sut.Skills);
        }

        [Fact]
        public void DoesNotAcceptNullSkill()
        {
            var sut = _fixture.Create<SkillSetModel>();

            Assert.Throws<ArgumentNullException>(()=> sut.Add(model: null));

        }

    }
}
