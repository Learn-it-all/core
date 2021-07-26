using AutoFixture;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.Summaries
{
    public class CreatingSummaryShould : Test
    {
        public CreatingSummaryShould()
        {

        }

        [Fact]
        public void ZeroAllLevelCountersGivenNoSpecificValueIsProvided()
        {
            var sut = _fixture.Create<Summary>();

            Assert.Equal<int>(0, sut.Unfamiliar);
            Assert.Equal<int>(0, sut.Novice);
            Assert.Equal<int>(0, sut.AdvancedBeginner);
            Assert.Equal<int>(0, sut.Competent);
            Assert.Equal<int>(0, sut.Proficient);
            Assert.Equal<int>(0, sut.Expert);
            Assert.Equal<int>(0, sut.Unknown);
        }

        [Theory]
        [InlineData(-1)]
        //[InlineData(0)]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //[InlineData(4)]
        //[InlineData(5)]
        public void NotifyLevelChangeGivenAdditionIsUsed(int targetLevel)
        {
            var sut = _fixture.Create<Summary>();
            var expectedEventArgs = new SummaryChangedEventArgs(targetLevel, 1);
            
            var actualEventArgs = Assert.Raises<SummaryChangedEventArgs>(
                a => sut.RaiseChangeEvent += a,
                a => sut.RaiseChangeEvent -= a,
                //Act
                () => sut.AddOneTo(targetLevel));

            Assert.NotNull(actualEventArgs);
        }
    }
}


