﻿using AutoFixture;
using Mtx.LearnItAll.Core.Calculations;
using Tests;
using Xunit;

namespace Summaries
{
    public class UpdatingSummaryShould : Test
    {
        public UpdatingSummaryShould()
        {

        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void RecalculateLevelGivenOneIsAdded(int expectedLevel)
        {
            var sut = _fixture.Create<Summary>();

            sut.AddOneTo(expectedLevel);

            Assert.Equal(1, sut.ValueOf(expectedLevel));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void RecalculateLevelGivenOneIsSubtracted(int expectedLevel)
        {
            var sut = _fixture.Create<Summary>();
            sut.AddOneTo(expectedLevel);

            sut.SubtractOneFrom(expectedLevel);

            Assert.Equal(0, sut.ValueOf(expectedLevel));
        }

    }
}


