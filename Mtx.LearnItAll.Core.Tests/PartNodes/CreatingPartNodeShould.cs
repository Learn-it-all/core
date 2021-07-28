using Mtx.LearnItAll.Core.Blueprints;
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

        [Fact]
        public void Issue()
        {
            var a = new SummaryA();
            var b = new SummaryA();
            Assert.Equal(a,b);
        }


    }

    public record SummaryA
    {
        public event EventHandler<SummaryChangedEventArgs> RaiseChangeEvent;
        public virtual Counter Unknown { get; private set; } = new UnknownCounter();
        public SummaryA()
        {
            //Unknown.RaiseChangeEvent += Unknown_RaiseChangeEvent;
        }

        private void Unknown_RaiseChangeEvent(object sender, SummaryChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
