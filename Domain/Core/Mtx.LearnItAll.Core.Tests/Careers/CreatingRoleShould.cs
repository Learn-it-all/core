using Xunit;
using AutoFixture;
using Mtx.LearnItAll.Core.Common;

namespace Mtx.LearnItAll.Core.Tests.Careers
{
    public class CreatingRoleShould : Test
    {
        [Fact]
        public void BeCreatedGivenNameIsValid()
        {
            _ = new Role(_fixture.Create<Name>());
        }
    }
}
