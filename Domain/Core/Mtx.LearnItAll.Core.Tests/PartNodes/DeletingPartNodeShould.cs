using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Linq;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.PartNodes
{
    public class DeletingPartNodeShould : Test
    {
        public DeletingPartNodeShould()
        {

        }

        [Fact]
        public void DeleteItGivenItExists()
        {
            var sut = _fixture.Create<PartNode>();
            var node = _fixture.Create<PartNode>();
            sut.Add(node);
            var expectedDeleteResult = DeletePartResult.Success(node.Name);
            var cmd = new DeletePartCmd(sut.Id,node.Id);
            //Act
            var actualResult = sut.TryDeletePart(cmd, out DeletePartResult actualDeleteResult);

            Assert.True(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.DoesNotContain(sut.Nodes, x => x.Name.Equals(node.Name));
        }

        [Fact]
        public void UpdateSummaryGivenPartNodeIsDeleted()
        {
            var sut = _fixture.Create<PartNode>();
            var expectedSummary = new Summary();

            var node = _fixture.Create<PartNode>();
            node.Add(new AddPartCmd(new Name("name"), node.Id));
            sut.Add(node);
            var cmd = new DeletePartCmd(sut.Id, node.Id);


            //Act
            _ = sut.TryDeletePart(cmd, out DeletePartResult _);

            Assert.Equal(expectedSummary,sut.Summary);
        }

        [Fact]
        public void FailGivenItDoesNOTExistAndThereAreNoFurtherPartNodesToLookInto()
        {
            var sut = _fixture.Create<PartNode>();
            var expectedDeleteResult = DeletePartResult.FailureForPartNotFound;
            var cmd = _fixture.Create<DeletePartCmd>();
            //Act
            var actualResult = sut.TryDeletePart(cmd, out DeletePartResult actualDeleteResult);

            Assert.False(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
        }

        [Fact]
        public void FailGivenItDoesNOTExistOnSutNorOnChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            sut.Add(child);
            var expectedDeleteResult = DeletePartResult.FailureForPartNotFound;
            var cmd = _fixture.Create<DeletePartCmd>();

            //Act
            var actualResult = sut.TryDeletePart(cmd, out DeletePartResult actualDeleteResult);

            Assert.False(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.Empty(sut.Parts);
        }

        [Fact]
        public void DeleteItGivenItExistisOnGrandChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            var grandChild = _fixture.Create<PartNode>();
            var toDelete = _fixture.Create<PartNode>();
            grandChild.Add(toDelete);
            child.Add(grandChild);
            sut.Add(child);
            var expectedDeleteResult = DeletePartResult.Success(toDelete.Name);
            var cmd = new DeletePartCmd(sut.Id, toDelete.Id);

            //Act
            var actualResult = sut.TryDeletePart(cmd, out DeletePartResult actualDeleteResult);

            Assert.True(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.DoesNotContain(grandChild.Parts, x => x.Name.Equals(toDelete.Name));
        }

        [Fact]
        public void UpdateSummaryGivenPartIsDeletedFromChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            var toDelete = _fixture.Create<PartNode>();
            child.Add(toDelete);
            sut.Add(child);
            var expectedSummary = sut.Summary;
            var cmd = new DeletePartCmd(sut.Id, toDelete.Id);

            //Act
            _ = sut.TryDeletePart(cmd, out DeletePartResult _);

            Assert.Equal(expectedSummary, sut.Summary);
        }

        [Fact]
        public void TurnPartNodeIntoPartGivenItsDeletedPartWasTheLastChild()
        {
            var sut = _fixture.Create<PartNode>();
            var childToBecomePartAfterDeletion = _fixture.Create<PartNode>();
            var toDelete = _fixture.Create<Part>();
            childToBecomePartAfterDeletion.Add(toDelete);
            sut.Add(childToBecomePartAfterDeletion);
            var expectedSummary = new Summary();
            expectedSummary.AddOneTo(SkillLevel.Unknown);
            var cmd = new DeletePartCmd(sut.Id, toDelete.Id);

            //Act
            _ = sut.TryDeletePart(cmd, out DeletePartResult _);

            Assert.Equal(expectedSummary, sut.Summary);
            Assert.DoesNotContain(sut.Nodes, x => x.Id == childToBecomePartAfterDeletion.Id);
            Assert.Contains(sut.Parts, x => x.Id == childToBecomePartAfterDeletion.Id);
        }

        [Fact]
        public void TurnPartNodeIntoPartGivenItsDeletedPartNodeWasTheLastChild()
        {

        }
    }
}
