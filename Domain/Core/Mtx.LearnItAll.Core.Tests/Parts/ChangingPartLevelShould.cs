using AutoFixture;
using Mtx.LearnItAll.Core;
using Mtx.LearnItAll.Core.Blueprints;
using Tests;
using Xunit;

namespace Parts
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
            Assert.Equal(expected, actual);

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
            Assert.Equal<int>(expected, actual);

        }
    }
}
