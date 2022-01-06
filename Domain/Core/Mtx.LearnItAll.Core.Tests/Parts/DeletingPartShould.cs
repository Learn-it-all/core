using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Linq;
using Xunit;

namespace Mtx.LearnItAll.Core.Tests.Parts
{
    public class DeletingPartShould : Test
    {
        public DeletingPartShould ()
        {

        }

        [Fact]
        public void DeleteItGivenItExistis()
        {
            var sut = _fixture.Create<PartNode>();
            var cmd = new AddPartCmd(new Name("name"), sut.Id);
            sut.TryAdd(cmd, out AddPartResult dummyResult);
            var idOfPartToDelete = dummyResult.IdOfAddedPart;
            var expectedDeleteResult = DeletePartResult.Success(cmd.Name);
            var deleteCmd = new DeletePartCmd(sut.Id, idOfPartToDelete);

            //Act
            var actualResult = sut.TryDeletePart(deleteCmd, out DeletePartResult actualDeleteResult);

            Assert.True(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.DoesNotContain(sut.Parts, x => x.Name.Equals(cmd.Name));
        }

        [Fact]
        public void UpdateSummaryGivenPartIsDeleted()
        {
            var sut = _fixture.Create<PartNode>();
            var expectedSummary = new Summary();

            var cmd = new AddPartCmd(new Name("name"), sut.Id);
            sut.TryAdd(cmd, out AddPartResult _);
            var deleteCmd = new DeletePartCmd(sut.Id, sut.Parts.First().Id);

            //Act
            _ = sut.TryDeletePart(deleteCmd, out DeletePartResult _);

            Assert.Equal(expectedSummary,sut.Summary);
        }

        [Fact]
        public void FailGivenItDoesNOTExistAndThereAreNoFurtherPartNodesToLookInto()
        {
            var sut = _fixture.Create<PartNode>();
            var expectedDeleteResult = DeletePartResult.FailureForPartNotFound;
            var deleteCmd = new DeletePartCmd(sut.Id, Guid.NewGuid());

            //Act
            var actualResult = sut.TryDeletePart(deleteCmd, out DeletePartResult actualDeleteResult);

            Assert.False(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.Empty(sut.Parts);
        }

        [Fact]
        public void FailGivenItDoesNOTExistOnSutNorOnChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            sut.Add(child);
            var expectedDeleteResult = DeletePartResult.FailureForPartNotFound;
            var deleteCmd = new DeletePartCmd(sut.Id, Guid.NewGuid());

            //Act
            var actualResult = sut.TryDeletePart(deleteCmd, out DeletePartResult actualDeleteResult);

            Assert.False(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.Empty(sut.Parts);
        }

        [Fact]
        public void DeleteItGivenItExistisOnChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            sut.Add(child);
            var cmd = new AddPartCmd(new Name("name"), child.Id);
            sut.TryAdd(cmd, out AddPartResult dummyResult);
            var idOfPartToDelete = dummyResult.IdOfAddedPart;
            var expectedDeleteResult = DeletePartResult.Success(cmd.Name);
            var deleteCmd = new DeletePartCmd(sut.Id, idOfPartToDelete);

            //Act
            var actualResult = sut.TryDeletePart(deleteCmd, out DeletePartResult actualDeleteResult);

            Assert.True(actualResult);
            Assert.Equal(expectedDeleteResult, actualDeleteResult);
            Assert.DoesNotContain(child.Parts, x => x.Name.Equals(cmd.Name));
        }

        [Fact]
        public void UpdateSummaryGivenPartIsDeletedFromChildPartNode()
        {
            var sut = _fixture.Create<PartNode>();
            var child = _fixture.Create<PartNode>();
            sut.Add(child);
            var expectedSummary = sut.Summary;

            var cmd = new AddPartCmd(new Name("name"), child.Id);
            var deleteCmd = new DeletePartCmd(sut.Id, child.Id);

            sut.TryAdd(cmd, out AddPartResult _);

            //Act
            _ = sut.TryDeletePart(deleteCmd, out DeletePartResult _);

            Assert.Equal(expectedSummary, sut.Summary);
        }
    }
}
