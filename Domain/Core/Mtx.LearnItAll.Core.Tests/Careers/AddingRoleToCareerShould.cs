using Xunit;
using AutoFixture;
using System.Linq;
using Mtx.LearnItAll.Core.Common;

namespace Mtx.LearnItAll.Core.Tests.Careers
{

    public class AddingRoleToCareerShould : Test
    {
        [Fact]
        public void AcceptRoleGivenItIsNotInUse()
        {
            var sut = _fixture.Create<Career>();
            var dummyRole = _fixture.Create<Role>();

            sut.Add(role: dummyRole);

            Assert.Contains(dummyRole,sut.Roles);
        }

        [Fact]
        public void IgnoreAddingSameRoleGivenItIsAlreadyIncluded()
        {
            var sut = _fixture.Create<Career>();
            var dummyRole = _fixture.Create<Role>();
            sut.Add(role: dummyRole);

            sut.Add(role: dummyRole);

            Assert.Equal(1,sut.Roles.Count);
        }

        [Fact(Skip = "yet to be implemented")]
        public void IgnoreAddingRoleGivenAnEqualRoleWasAdded()
        {
            var dummyName = _fixture.Create<Name>();
             _fixture.Register<Name>(() => dummyName);//name will be always the same accross different Roles

            var sut = _fixture.Create<Career>();
            var dummyRole = _fixture.Create<Role>();
            var anotherDummyRole = _fixture.Create<Role>();
            sut.Add(role: dummyRole);

            sut.Add(role: anotherDummyRole);

            Assert.Equal(1, sut.Roles.Count);
            Assert.Same(dummyRole, sut.Roles.First());
        }



    }
}
