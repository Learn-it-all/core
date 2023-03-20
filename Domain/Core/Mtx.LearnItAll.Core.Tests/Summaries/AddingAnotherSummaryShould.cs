using AutoFixture;
using Mtx.LearnItAll.Core.Calculations;
using Tests;
using Xunit;

namespace Summaries
{
    public class AddingAnotherSummaryShould : Test
    {
        public AddingAnotherSummaryShould()
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
            var toBeAdded = _fixture.Create<Summary>();
            toBeAdded.AddOneTo(expectedLevel);

            var sut = _fixture.Create<Summary>();
            

            sut.Add(another: toBeAdded);

            Assert.Equal(toBeAdded, sut);
        }


    }
}


