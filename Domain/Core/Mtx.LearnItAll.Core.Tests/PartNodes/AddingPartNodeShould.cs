using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Calculations;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;
using System;
using System.Linq;
using Tests;
using Xunit;

namespace PartNodes
{
    public class AddingPartNodeShould : Test
    {
        public AddingPartNodeShould()
        {

        }

        [Fact]
        public void AddAsChildGivenNameIsNotInUseByOtherChild()
        {
            var dummySkill = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();

            sut.Add(newNode: dummySkill);

            Assert.Contains(dummySkill, sut.Nodes);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionGivenNameIsInUseByDirectChildren()
        {
            var dummySkill = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();
            var expectedErrorMessage = string.Format(CoreMessages.SkillModel_CannotAddDuplicateNameForChildOnSameLevel, dummySkill.Name, sut.Name);

            sut.Add(newNode: dummySkill);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    sut.Add(newNode: dummySkill);

                }
                catch (InvalidOperationException ioe)
                {
                    Assert.Equal(expectedErrorMessage, ioe.Message);
                    throw;
                }
            });
        }

        [Fact]
        public void NeverThrowGivenPartNodeNameIsInUseByDirectChildWhenTryingToAdd()
        {
            var dummySkill = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();
            sut.Add(newNode: dummySkill);

           var actual = sut.TryAdd(sut.Id,dummySkill);

            Assert.False(actual);
        }
        [Fact]
        public void AddSkillAsLeafGivenParentIdIsChild()
        {
            var children = _fixture.CreateMany<PartNode>(3);
            var expectedParent = children.ElementAt(1);

            var grandChild = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();
            foreach (var skill in children)
            {
                sut.Add(skill);
            }

            var actual = sut.TryAdd(parentId: expectedParent.Id, grandChild);

            Assert.True(actual, "Should have added newNode");
            Assert.Contains(grandChild, expectedParent.Nodes);

        }

        [Fact]
        public void AddSkillAsLeafGivenParentIdIsGrandGrandGrandChild()
        {
            var expectedParent = _fixture.Create<PartNode>();
            var sutGrandChild = _fixture.Create<PartNode>();
            sutGrandChild.Add(expectedParent);
            var sutChild = _fixture.Create<PartNode>();
            sutChild.Add(sutGrandChild);
      
            var dummyModel = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();
            sut.Add(sutChild);
        
            var actual = sut.TryAdd(expectedParent.Id,dummyModel);

            Assert.True(actual, "Should have added SkillBluePrint");
            Assert.Contains(dummyModel, expectedParent.Nodes);

        }

        [Fact]
        public void AddSkillAsSUTsChildGivenParentIdIsTheSUTItself()
        {
            var child = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();

            var actual = sut.TryAdd(sut.Id, child);

            Assert.True(actual, "Should have added SkillBluePrint");
            Assert.Contains(child, sut.Nodes);
        }

        [Fact]
        public void AddSkillAsSUTsChildGivenNoParentIdIsProvided()
        {
            var child = _fixture.Create<PartNode>();
            var sut = _fixture.Create<PartNode>();

            sut.Add(child);

            Assert.Contains(child, sut.Nodes);
        }

        [Fact]
        public void ReturnFalseGivenThereIsNoMatchingSkillAsDirectChild()
        {
            var dummy = _fixture.Create<PartNode>();
            var dummyId = _fixture.Create<Guid>();
            var sut = _fixture.Create<PartNode>();

            var actual = sut.TryAdd(dummyId, dummy);

            Assert.False(actual, "Should not add a SkillModel to a non existing parent");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void RegisterSummaryToReceiveUpdatesFromChildNodeGivenNodeIsAdded(int expectedLevel)
        {
            var sut = _fixture.Create<PartNode>();
            var expectedCounter = Counter.Create(expectedLevel);
            expectedCounter.AddOne();

            var childNode = _fixture.Create<PartNode>();
            Name name = _fixture.Create<Name>();
            var cmd = new AddPartCmd(name, childNode.Id);
            childNode.Add(cmd);
            _ = sut.TryAdd(sut.Id, childNode);

            sut.ChangeLevel(name, childNode.Id, expectedLevel);

            var actualCounter = sut.Summary.CounterFor(expectedLevel);
            Assert.Equal(expectedCounter, actualCounter);


        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void UpdateSummaryGivenNodeIsAdded(int expectedLevel)
        {
            var sut = _fixture.Create<PartNode>();
            var expectedCounter = Counter.Create(expectedLevel);
            expectedCounter.AddOne();

            var childNode = _fixture.Create<PartNode>();
            Name name = _fixture.Create<Name>();
            var cmd = new AddPartCmd(name, childNode.Id);
            childNode.Add(cmd);
            childNode.ChangeLevel(name, childNode.Id, expectedLevel);

            _ = sut.TryAdd(sut.Id, childNode);


            var actualCounter = sut.Summary.CounterFor(expectedLevel);
            Assert.Equal(expectedCounter, actualCounter);


        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void UpdateSummaryGivenPartIsAddedToAlreadyExistingNode(int expectedLevel)
        {
            var sut = _fixture.Create<PartNode>();
            var expectedCounter = Counter.Create(expectedLevel);
            expectedCounter.AddOne();
            expectedCounter.AddOne();

            var childNode = _fixture.Create<PartNode>();
            Name name = _fixture.Create<Name>();
            var cmd = new AddPartCmd(name, childNode.Id);
            childNode.Add(cmd);
            childNode.ChangeLevel(name, childNode.Id, expectedLevel);
            sut.Add(childNode);

            sut.Add(new AddPartCmd(new Name("new part"), childNode.Id));
            childNode.ChangeLevel("new part", childNode.Id, expectedLevel);


            var actualCounter = sut.Summary.CounterFor(expectedLevel);
            Assert.Equal(expectedCounter, actualCounter);


        }


    }
}
