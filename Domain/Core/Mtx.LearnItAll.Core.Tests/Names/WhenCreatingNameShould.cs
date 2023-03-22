using AutoFixture;
using Mtx.LearnItAll.Core.Common;
using System;
using System.Linq;
using Tests;
using Xunit;

namespace Names
{
    public class WhenCreatingNameShould : Test

    {
        public WhenCreatingNameShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<Name>();

        }

        [Fact]
        public void EnforceMaxLength()
        {
            string outOfRangeName = new(Enumerable.Repeat('a', 51).ToArray());

            Assert.Throws<ArgumentOutOfRangeException>(() => new Name(outOfRangeName));

        }
    }
}
