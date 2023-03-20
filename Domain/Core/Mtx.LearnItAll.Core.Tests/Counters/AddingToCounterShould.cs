using Mtx.LearnItAll.Core;
using Mtx.LearnItAll.Core.Calculations;
using Tests;
using Xunit;

namespace Counters
{
    public class AddingToCounterShould : Test
    {

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(0, 2, 2)]
        [InlineData(0, 3, 3)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 3, 2)]
        [InlineData(2, 3, 1)]
        [InlineData(2, 4, 2)]
        public void RaiseEventWithExpectedDifferenceGivenNewValueIsBiggerThanCurrent(int initialValue, int newValue, int expectedDifference)
        {
            var sut = new Counter(SkillLevel.Unknown, initialValue);
            var expectedEvent = new SummaryChangedEventArgs(SkillLevel.Unknown, expectedDifference);

            var actualEvent = Assert.Raises<SummaryChangedEventArgs>(
               a => sut.RaiseChangeEvent += a,
               a => sut.RaiseChangeEvent -= a,
               //Act
               () => sut.Current = newValue);

            Assert.Equal(expectedEvent, actualEvent.Arguments); ;


        }

        [Theory]
        [InlineData(1, 0, -1)]
        [InlineData(2, 1, -1)]
        [InlineData(2, 0, -2)]
        [InlineData(3, 0, -3)]
        public void RaiseEventWithExpectedDifferenceGivenNewValueIsSmalllerThanCurrent(int initialValue, int newValue, int expectedDifference)
        {
            var sut = new Counter(SkillLevel.Unknown, initialValue);
            var expectedEvent = new SummaryChangedEventArgs(SkillLevel.Unknown, expectedDifference);

            var actualEvent = Assert.Raises<SummaryChangedEventArgs>(
               a => sut.RaiseChangeEvent += a,
               a => sut.RaiseChangeEvent -= a,
               //Act
               () => sut.Current = newValue);

            Assert.Equal(expectedEvent, actualEvent.Arguments);
        }

        [Fact]
        public void KeepCounterAsWholeNumberGivenNewValueIsSmallerThanZero()
        {
            var sut = new Counter(SkillLevel.Unknown, 0);

            sut.Current = -1;

            Assert.Equal(0, sut.Current);
        }
    }
}
