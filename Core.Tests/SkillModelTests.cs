using AutoFixture;
using Core.Domain;
using Mtx.Common.Domain;
using Xunit;

namespace Core.Tests
{
    public class SkillModelTests
    {
        private Fixture _fixture => new Fixture();

        public SkillModelTests()
        {

        }

        [Fact]
        public void Exists()
        {
            _ = _fixture.Create<SkillModel>();
        }

        [Fact]
        public void HasUniqueId()
        {
            var sut = _fixture.Create<SkillModel>();
            var dummy = _fixture.Create<SkillModel>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }

        [Fact]
        public void IsEntity()
        {
            var sut = _fixture.Create<SkillModel>();

            Assert.IsAssignableFrom<Entity>(sut);
        }
    }
}
