using AutoFixture;
using Mtx.LearnItAll.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests;
using Xunit;
using Xunit.Sdk;

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
        [Fact]
        public void X()
        {
            Func<Name,string> f = (name) => name;
            Add(f);
            Name name = new("a");
            var atual = diction[typeof(Name)].Invoke(name);
            Assert.Equal("a",atual);
        }
	public Dictionary<Type, dynamic> diction = new Dictionary<Type, dynamic>();
	public void Add<T>(Func<T, string> func) where T : class
	{
		diction[typeof(T)] = func;
	}

    }
}
