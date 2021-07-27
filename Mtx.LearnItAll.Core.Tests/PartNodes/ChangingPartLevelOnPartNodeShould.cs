using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Common;
using Xunit;
using Mtx.LearnItAll.Core.Calculations;

namespace Mtx.LearnItAll.Core.Tests.PartNodes
{
    public class ChangingPartLevelOnPartNodeShould : Test
    {
        public ChangingPartLevelOnPartNodeShould()
        {

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void UpdateSummaryGivenPartLevelIsUpdated(int expected)
        {
            var sut = _fixture.Create<PartNode>();
            var expectedSummary = _fixture.Create<Summary>();
            expectedSummary.AddOneTo(expected);

            var cmd = new AddPartCmd(new Name("name"), sut.Id);
            sut.Add(cmd);

            sut.ChangeLevel(partName: cmd.Name, parentId: sut.Id, expected);

            int actual = sut.Parts.First(x => x.Name.Equals(cmd.Name)).Level;

            Assert.Equal(expected, actual);
            Assert.Equal(expectedSummary, sut.Summary);
        }
        [Fact]
        public void UpdateSummaryGivenPartNodeIsAddedAsChild()
        { 
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void DelegateToChildPartNodeGivenParentIdDoesNotMatchWithSuts(int expected)
        {
            var sut = _fixture.Create<PartNode>();
            var childPartNodes = _fixture.CreateMany<PartNode>().ToList();
            var partNodeToDelegateTo = childPartNodes[1];
            var cmd = new AddPartCmd(new Name("name"), partNodeToDelegateTo.Id);
            partNodeToDelegateTo.Add(cmd);
            childPartNodes.ForEach(x => sut.TryAdd(sut.Id, x));

            sut.ChangeLevel(cmd.Name, partNodeToDelegateTo.Id, expected);

            int actual = partNodeToDelegateTo.Parts.First(x => x.Name.Equals(cmd.Name)).Level;

            Assert.Equal(expected, actual);


        }
    }
}
