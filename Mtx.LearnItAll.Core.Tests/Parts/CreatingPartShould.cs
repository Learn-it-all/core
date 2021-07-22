using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.Parts
{
    public class CreatingPartShould : Test
    {
        [Fact]
        public void BeUnknownLevelGivenNoLevelIsProvided()
        {
            var sut = _fixture.Create<Part>();
            int actual = sut.Level;
            SkillLevel actualDescriptive = sut.DescriptiveLevel;
            Assert.Equal(SkillLevel.Unknown.Number, actual);
            Assert.Equal(SkillLevel.Unknown, actualDescriptive);

        }

        [Fact]
        public void SetInitialDataGivenUponConstruction()
        {

        }
    }
}
