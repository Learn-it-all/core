using AutoFixture;
using Mtx.Common.Domain;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common;
using System;
using System.Linq;
using Tests;
using Xunit;

namespace Blueprints
{
    public class CreatingNewSkillBlueprintShould : Test
    {

        [Fact]
        public void AddAsChildOfRootSkillGivenThereIsNoNamingConflict()
        {
            var sut = _fixture.Create<PersonalSkill>();
            var dummy = _fixture.Create<PartNode>();
            sut.Add(dummy);
            Assert.Contains(dummy, sut.Nodes);
        }

        [Fact(Skip = "to be implemented")]
        public void AllowNoMoreThan2000SInnerkillModels()
        {

        }
    }

    public class CreatingSkillShould
    {
        [Fact]
        public void SkillCreatedEventKept()
        {
            var now = DateTimeOffset.Now;
            var id = UniqueId.New();
            var name = new Name("C#");
			var skill = SkillBluePrint.Create(id, name, now);
            Assert.Single(skill.Applied);
            Assert.Equal("C#", skill.Name);
            Assert.Equal(SkillCreated.With(id, name, now), skill.Applied.First());

        }
    }
}