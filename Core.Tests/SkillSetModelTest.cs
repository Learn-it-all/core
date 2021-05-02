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
        public void HasAtLeastOneSkill()
        {
            var expectedSkills = new List<SkillModel>();

            Assert.Throws<ArgumentException>("skills",() => _fixture.Get((List<SkillModel> _,SkillSetModelName name) => new SkillSetModel(expectedSkills,name)));

        }

        [Fact]
        public void CanHaveMoreThanOneSkill()
        {
            var expectedSkills = _fixture.CreateMany<SkillModel>();
            var sut = _fixture.Get((List<SkillModel> l,SkillSetModelName name) => new SkillSetModel(expectedSkills,name));

            Assert.Equal(expectedSkills, sut.Skills);
        }

        [Fact]
        public void DoesNotAcceptNullListOfSkills()
        {
            Assert.Throws<ArgumentNullException>(() => new SkillSetModel((List<SkillModel>)null,null));
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
            var sut = _fixture.Get((List<SkillModel> models,SkillSetModelName _) => new SkillSetModel(models,expected));

            string actual = sut.Name;
            Assert.Equal(expected, actual);
        }


    }
}
