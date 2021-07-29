﻿using Mtx.LearnItAll.Core.Blueprints;
using Xunit;
using AutoFixture;
using Mtx.LearnItAll.Core.Calculations;
using System;

namespace Mtx.LearnItAll.Core.Tests.PartNodes
{
    public class CreatingPartNodeShould : Test
    {
        public CreatingPartNodeShould()
        {

        }

        [Fact]
        public void StartWithEmptySummary()
        {
            var sut = _fixture.Create<PartNode>();

            var actual = sut.Summary;

            Assert.Equal(new Summary(), actual);
        }
    }
}
