using AutoFixture;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace Core.Tests
{
    public class SkillSetTest
    {
        private Fixture _fixture => new Fixture();

        public SkillSetTest()
        {

        }

        [Fact]
        public void Exists()
        {
            var sut = _fixture.Create<SkillSet>();

        }

        [Fact]
        public void SkillsAreNeverNull()
        {
            var sut = _fixture.Create<SkillSet>();

            Assert.NotNull(sut.Skills);
        }


        [Fact]
        public void HasAtLeastOneSkill()
        {
            var expectedSkill = _fixture.Create<Skill>();

            var sut = _fixture.Get((Skill _) => new  SkillSet(expectedSkill));

            IReadOnlyList<Skill> actualSkills = sut.Skills;

            Assert.Single(actualSkills);
            Assert.Contains(expectedSkill,sut.Skills);
        }

        [Fact]
        public void CanHaveMoreThanOneSkill()
        {
            var expectedSkills = _fixture.CreateMany<Skill>();
            var sut = _fixture.Get((List<Skill> l) => new SkillSet(expectedSkills));

            Assert.Equal(expectedSkills, sut.Skills);
        }

        [Fact]
        public void DoesNotAcceptNullListOfSkills()
        {
           Assert.Throws<ArgumentNullException>(()=> new SkillSet((List<Skill>)null));

        }

        [Fact]
        public void DoesNotAcceptNullSkill()
        {
            Assert.Throws<ArgumentNullException>(() => new SkillSet((Skill)null));

        }
    }
}
