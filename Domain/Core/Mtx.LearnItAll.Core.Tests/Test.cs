using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using System;
using Xunit;

namespace Tests
{
    [Trait("Domain","Core")]
    public abstract class Test
    {
        protected Fixture _fixture = new();
        public Test()
        {
            _fixture.Register(() => new SkillBlueprint(_fixture.Create<Name>()));
            _fixture.Register(() => new PartNode(_fixture.Create<Name>()));
            _fixture.Register(() => new Part(_fixture.Create<Name>(), Guid.Empty));
            _fixture.Register(() => new Summary());
        }

    }
}