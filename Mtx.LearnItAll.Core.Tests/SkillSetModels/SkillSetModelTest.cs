using AutoFixture;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.SkillSetModels
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
            var expected = _fixture.Create<ModelName>();
            var sut = _fixture.Get((ModelName _) => new SkillSetModel(expected));

            string actual = sut.Name;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddSkill()
        {
            var dummySkill = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillSetModel>();

            sut.AddNew(skill: dummySkill);

            Assert.True(sut.Skills.Count == 1);
            Assert.Contains(dummySkill, sut.Skills);
        }

        [Fact]
        public void AddNewSkill_WhenSkillAlreadyIncluded_Throws()
        {
            var dummySkill = _fixture.Create<SkillModel>();
            var sut = _fixture.Create<SkillSetModel>();
            sut.AddNew(skill: dummySkill);
            var expectedExceptionMessage = string.Format(Messages.SkillModel_ASkillWithSameNameAlreadyExistis, dummySkill.Name, sut.Name);

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    sut.AddNew(dummySkill);
                }
                catch (InvalidOperationException ioe)
                {
                    Assert.Equal(expectedExceptionMessage, ioe.Message);
                    throw;
                }
            });
        }

        [Fact]
        public void CannotHaveMoreThan50DirectChildSkillModels()
        {
            List<SkillModel> dummySkillModels = new(_fixture.CreateMany<SkillModel>(51));
            var sut = _fixture.Create<SkillSetModel>();

            var expectedExceptionMessage = string.Format(Messages.SkillModelSet_MaximumDirectSkillModelChildExceeded, sut.MaximumDirectSkillModelChild);


            Assert.Throws<InvalidOperationException>(() =>
            {

                try
                {
                    dummySkillModels.ForEach(x => sut.AddNew(x));

                }
                catch (InvalidOperationException ioe)
                {
                    Assert.Equal(expectedExceptionMessage, ioe.Message);
                    throw;
                }
            });


        }

    }
}
