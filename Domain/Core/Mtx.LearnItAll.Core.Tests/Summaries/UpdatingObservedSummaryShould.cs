using AutoFixture;
using Mtx.LearnItAll.Core.Calculations;
using SemanticComparison.Fluent;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.Summaries
{
    public class UpdatingObservedSummaryShould : Test
    {
        public UpdatingObservedSummaryShould()
        {

        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        public void RecalculateLevelsGivenAnObservedSummaryIsUpdated(int expectedLevel, int expectedValue)
        {
            var sut = _fixture.Create<Summary>();

            var dummyArgs = new SummaryChangedEventArgs(expectedLevel, 1);

            sut.RecalculateOnChange(null, dummyArgs);

            Assert.Equal(expectedValue, sut.ValueOf(expectedLevel));
        }

    }
}


