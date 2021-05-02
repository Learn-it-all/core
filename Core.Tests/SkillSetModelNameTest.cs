using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Core.Domain;
using Xunit;

namespace Core.Tests
{
    public class SkillSetModelNameTest

    {
        private Fixture _fixture => new Fixture();

        public SkillSetModelNameTest()
        {

        }

        [Fact]
        public void Exists()
        {
            _ = _fixture.Create<SkillSetModelName>();

        }

        [Fact]
        public void NameHasNoMoreThan50Chars()
        {
            string outOfRangeName = new(Enumerable.Repeat('a', 51).ToArray());

            Assert.Throws<ArgumentOutOfRangeException>(() => new SkillSetModelName(outOfRangeName));

        }

    }
}
