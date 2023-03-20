using Xunit;
using AutoFixture;
using Mtx.LearnItAll.Core.Common;
using Tests;
using Mtx.LearnItAll.Core;

namespace Careers
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
