﻿using AutoFixture;
using Mtx.LearnItAll.Core;
using Mtx.LearnItAll.Core.Blueprints;
using System;
using Tests;
using Xunit;

namespace Parts
{
    public class CreatingPartShould : Test
    {

        public CreatingPartShould()
        {

        }

        [Fact]
        public void Exist()
        {
            _ = _fixture.Create<Part>();
        }

        [Fact]
        public void HaveUniqueId()
        {
            var sut = _fixture.Create<Part>();
            var dummy = _fixture.Create<Part>();

            Assert.NotEqual(dummy.Id, sut.Id);

        }


        [Fact]
        public void SetCreationDateToNow()
        {
            var sut = _fixture.Create<Part>();

            DateTime actual = sut.Created;

            Assert.Equal(DateTime.Today, actual.Date);
        }

        [Fact]
        public void BeUnknownLevelGivenNoLevelIsProvided()
        {
            var sut = _fixture.Create<Part>();
            int actual = sut.Level;
            Assert.Equal(SkillLevel.Unknown.Id, actual);

        }

        [Fact]
        public void SetInitialDataGivenUponConstruction()
        {

        }

    }

    
}
