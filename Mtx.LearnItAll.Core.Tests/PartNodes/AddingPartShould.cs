using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Linq;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.PartNodes
{
    public class AddingPartShould : Test
    {
        public AddingPartShould()
        {

        }

        [Fact]
        public void AddPartAsChildGivenParentIdMatchesWithSUTs()
        {
            var sut = _fixture.Create<PartNode>();
            var cmd = new AddPartCmd(new Name("name"), sut.Id);

            sut.Add(cmd);

            Assert.Contains(sut.Parts, x => x.Name.Equals(cmd.Name));
        }

        [Fact]
        public void FailToAddPartAsChildGivenNameIsAlreadyTaken()
        {
            var sut = _fixture.Create<PartNode>();
            var cmd = new AddPartCmd(new Name("name"), sut.Id);
            sut.Add(cmd);

            Assert.Throws<InvalidOperationException>(()=> sut.Add(cmd));

        }

        [Fact]
        public void DelegateNewPartToChildPartNodeGivenPartsParentIdDoesNotMatchWithSUTs()
        {
            var sut = _fixture.Create<PartNode>();
            var childPartNode = _fixture.Create<PartNode>();
            sut.Add(childPartNode);
            var cmd = new AddPartCmd(new Name("name"), childPartNode.Id);

            sut.Add(cmd);

            Assert.Contains(sut.Nodes.First().Parts, x => x.Name.Equals(cmd.Name));
        }

        [Fact]
        public void UpdateSummaryGivenPartIsAdded()
        {
            var sut = _fixture.Create<PartNode>();
            var cmd = new AddPartCmd(new Name("name"), sut.Id);
            var expectedSummary = new Summary();
            expectedSummary.AddOneTo(SkillLevel.Unknown);


            sut.Add(cmd);

            Summary actualSummary = sut.Summary;
            Assert.Equal(expectedSummary, actualSummary);

        }


        [Fact]
        public void NotifySummaryStateChangeGivenPartIsAdded()
        {
            var sut = _fixture.Create<PartNode>();
            var cmd = new AddPartCmd(new Name("name"), sut.Id);


            sut.Add(cmd);


        }

        [Fact]
        public void TurnExistigPartIntoPartNodeGivenParentIsAPart()
        {
            var sut = _fixture.Create<PartNode>();

            var cmd = new AddPartCmd(_fixture.Create<Name>(), sut.Id);
            sut.Add(cmd);
            var partToBeTurnedIntoNode = sut.Parts.First();
            cmd = new AddPartCmd(_fixture.Create<Name>(), partToBeTurnedIntoNode.Id);

            sut.Add(cmd);

            Assert.Collection(sut.Nodes, x => x.Name.Equals(partToBeTurnedIntoNode.Name));
            Assert.Empty(sut.Parts);

        }

    }
}
