using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.Parts
{
    public class ChangingPartLevelShould : Test
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void SetNewLevelGivenItIsInTheScale(int expected)
        {
            var sut = _fixture.Create<Part>();

            sut.ChangeLevel(expected);
            int actual = sut.Level;
            SkillLevel actualDescriptive = sut.DescriptiveLevel;
            Assert.Equal(expected, actual);
            Assert.Equal<SkillLevel>(expected, actualDescriptive);

        }

        [Theory]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void SetNewLevelToUnknownGivenItIsInOutsideTheScale(int outOfRange)
        {
            var sut = _fixture.Create<Part>();
            var expected = SkillLevel.Unknown;

            sut.ChangeLevel(outOfRange);
            int actual = sut.Level;
            SkillLevel actualDescriptive = sut.DescriptiveLevel;
            Assert.Equal<int>(expected, actual);
            Assert.Equal<SkillLevel>(expected, actualDescriptive);

        }
    }
}
