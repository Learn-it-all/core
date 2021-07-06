using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Linq;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillModels
{
    public class AddingSkillPartShould : Test
    {
        public AddingSkillPartShould()
        {

        }

        [Fact]
        public void AddAsChildGivenNameIsNotInUseByOtherChild()
        {
            var dummySkill = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();

            sut.Add(skill: dummySkill);

            Assert.Contains(dummySkill, sut.Skills);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionGivenNameIsInUseByDirectChildren()
        {
            var dummySkill = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();
            var expectedErrorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, dummySkill.Name, sut.Name);

            sut.Add(skill: dummySkill);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    sut.Add(skill: dummySkill);

                }
                catch (InvalidOperationException ioe)
                {
                    Assert.Equal(expectedErrorMessage, ioe.Message);
                    throw;
                }
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionGivenNameIsInUseByDirectChildWhenTryingToAdd()
        {
            var dummySkill = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();
            var expectedErrorMessage = string.Format(Messages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, dummySkill.Name, sut.Name);

            sut.Add(skill: dummySkill);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    sut.TryAdd(sut.Id,dummySkill);

                }
                catch (InvalidOperationException ioe)
                {
                    Assert.Equal(expectedErrorMessage, ioe.Message);
                    throw;
                }
            });
        }
        [Fact]
        public void AddSkillAsLeafGivenParentIdIsChild()
        {
            var children = _fixture.CreateMany<Part>(3);
            var expectedParent = children.ElementAt(1);

            var grandChild = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();
            foreach (var skill in children)
            {
                sut.Add(skill);
            }

            var actual = sut.TryAdd(parentId: expectedParent.Id, grandChild);

            Assert.True(actual, "Should have added skill");
            Assert.Contains(grandChild, expectedParent.Skills);

        }

        [Fact]
        public void AddSkillAsLeafGivenParentIdIsGrandGrandGrandChild()
        {
            var expectedParent = _fixture.Create<Part>();
            var sutGrandChild = _fixture.Create<Part>();
            sutGrandChild.Add(expectedParent);
            var sutChild = _fixture.Create<Part>();
            sutChild.Add(sutGrandChild);
      
            var dummyModel = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();
            sut.Add(sutChild);
        
            var actual = sut.TryAdd(expectedParent.Id,dummyModel);

            Assert.True(actual, "Should have added Skill");
            Assert.Contains(dummyModel, expectedParent.Skills);

        }

        [Fact]
        public void AddSkillAsSUTsChildGivenParentIdIsTheSUTItself()
        {
            var child = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();

            var actual = sut.TryAdd(sut.Id, child);

            Assert.True(actual, "Should have added Skill");
            Assert.Contains(child, sut.Skills);
        }

        [Fact]
        public void AddSkillAsSUTsChildGivenNoParentIdIsProvided()
        {
            var child = _fixture.Create<Part>();
            var sut = _fixture.Create<Part>();

            sut.Add(child);

            Assert.Contains(child, sut.Skills);
        }

        [Fact]
        public void ReturnFalseGivenThereIsNoMatchingSkillAsDirectChild()
        {
            var dummy = _fixture.Create<Part>();
            var dummyId = _fixture.Create<Guid>();
            var sut = _fixture.Create<Part>();

            var actual = sut.TryAdd(dummyId, dummy);

            Assert.False(actual, "Should not add a SkillModel to a non existing parent");
        }
    }
}
