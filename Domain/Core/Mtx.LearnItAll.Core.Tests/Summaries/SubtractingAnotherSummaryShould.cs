using AutoFixture;
using Mtx.LearnItAll.Core.Calculations;
using Tests;
using Xunit;

namespace Summaries
{
    public class SubtractingAnotherSummaryShould : Test
    {
        public SubtractingAnotherSummaryShould()
        {

        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void RecalculateLevelGivenOneLevelIsGreaterThanZero(int expectedLevel)
        {
            var toBeSubtracted = _fixture.Create<Summary>();
            toBeSubtracted.AddOneTo(expectedLevel);

            var sut = _fixture.Create<Summary>();
            sut.AddOneTo(expectedLevel);
            //now we sum them both on sut:
            sut.Add(toBeSubtracted); //so the sut has now 2 as value on the expected level

            //Act
            sut.Subtract(another: toBeSubtracted);

            Assert.Equal(toBeSubtracted, sut);
        }


    }
}


