using AutoFixture;
using System;
using System.Linq;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests
{
    public class ModelNameTest

    {
        private Fixture _fixture => new Fixture();

        public ModelNameTest()
        {

        }

        [Fact]
        public void Exists()
        {
            _ = _fixture.Create<ModelName>();

        }

        [Fact]
        public void Ctor_NameHasNoMoreThan50Chars()
        {
            string outOfRangeName = new(Enumerable.Repeat('a', 51).ToArray());

            Assert.Throws<ArgumentOutOfRangeException>(() => new ModelName(outOfRangeName));

        }

      

    }
}
