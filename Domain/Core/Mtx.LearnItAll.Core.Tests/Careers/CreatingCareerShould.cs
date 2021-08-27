﻿using Xunit;
using AutoFixture;
using Mtx.LearnItAll.Core.Common;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Tests.Careers
{
    public class CreatingCareerShould : Test
    {
        [Fact]
        public void BeCreatedGivenNameIsValid()
        {
            _ = new Career(new Name(_fixture.Create<string>()));
        }

        [Fact]
        public void GenerateIdGivenNameIsValid()
        {
            var sut = _fixture.Create<Career>();
            var dummy = _fixture.Create<Career>();

            Assert.NotEqual<Guid>(Guid.Empty, sut.Id);
            Assert.NotEqual<Guid>(dummy.Id, sut.Id);
        }
        [Fact]
        public void HasNoRolesGivenItIsNew()
        {
            var sut = _fixture.Create<Career>();
            IReadOnlyCollection<Role> actual = sut.Roles;

            Assert.Empty(actual);
        }
    }
}
