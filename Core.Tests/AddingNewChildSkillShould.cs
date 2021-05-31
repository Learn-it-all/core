using AutoFixture;
using Core.Domain;
using Core.Resources;
using System;
using System.Linq;
using Xunit;

namespace Core.Tests
{
    public class AddingNewChildSkillShould : Test
    {
        public AddingNewChildSkillShould()
        {

        }

        [Fact]
        public void AddAsChildGivenNameIsNotInUseByOtherChild()
        {
            var dummySkill = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillModel>();

            sut.Add(skill: dummySkill);

            Assert.Contains(dummySkill, sut.Skills);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionGivenNameIsInUseByDirectChildren()
        {
            var dummySkill = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillModel>();
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
        public void AddSkillAsLeafGivenParentIdExists()
        {
            var children = _fixture.CreateMany<SkillModel>(3);
            var expectedParent = children.ElementAt(1);

            var grandChild = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillModel>();
            foreach (var skill in children)
            {
                sut.Add(skill);
            }

            sut.Add(parentId: expectedParent.Id, grandChild);

            Assert.Contains(grandChild, expectedParent.Skills);

        }

        [Fact]
        public void AddSkillAsSUTChildGivenParentIdIsTheSUTItself()
        {
            var child = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillModel>();

            sut.Add(sut.Id, child);

            Assert.Contains(child, sut.Skills);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionGivenParentIdDoesNotExist()
        {
            var dummy = _fixture.Create<SkillModel>();
            var dummyId = _fixture.Create<Guid>();
            var sut = _fixture.Create<SkillModel>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    sut.Add(dummyId,dummy);

                }
                catch (InvalidOperationException ioe)
                {
                    //Assert.Equal(expectedErrorMessage, ioe.Message);
                    throw;
                }
            });
        }
    }
}
