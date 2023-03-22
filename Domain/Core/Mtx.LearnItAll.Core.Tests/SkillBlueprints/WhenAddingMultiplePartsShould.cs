using AutoFixture;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using Tests;
using Xunit;


namespace Skills;

public class WhenAddingMultiplePartsShould : Test
{ 
    [Fact]
    public void SucceedGivenParentIsExistingNode()
    {
        var sut = _fixture.Create<SkillBlueprint>();
        var names = _fixture.CreateMany<Name>();
        PartNode part = _fixture.Create<PartNode>();
        var cmd = new AddMultiplePartsCmd(names, part.Id, sut.RootPartId);
        sut.Add(part);

        //Act
        var actualResult = sut.Add(cmd);

        Assert.False(actualResult.HasErrors);
        foreach (var name in cmd.Names)
            Assert.True(actualResult.Succeeded(name), $"{name} was not added in the result");
    }

    [Fact]
    public void SucceedGivenParentIsExistingPart()
    {
        var sut = _fixture.Create<SkillBlueprint>();
        var names = _fixture.CreateMany<Name>();
        var addPartCmd = new AddPartCmd(_fixture.Create<Name>(), sut.RootPartId);
        sut.TryAdd(addPartCmd, out var partResult);
        var cmd = new AddMultiplePartsCmd(names, parentId: partResult.IdOfAddedPart,sut.RootPartId);

        //Act
        var actualResult = sut.Add(cmd);
        Assert.False(actualResult.HasErrors);
        foreach (var name in cmd.Names)
            Assert.True(actualResult.Succeeded(name), $"{name} was not added in the result");
    }

    [Fact]
    public void FailGivenPartNameIsDuplicated()
    {
        var sut = _fixture.Create<SkillBlueprint>();
        var names = _fixture.CreateMany<Name>();
        var addPartCmd = new AddPartCmd(_fixture.Create<Name>(), sut.RootPartId);
        sut.TryAdd(addPartCmd, out var partResult);
        var cmd = new AddMultiplePartsCmd(names, parentId: partResult.IdOfAddedPart, sut.RootPartId);
        _ = sut.Add(cmd);

        //Act
        var actualResult = sut.Add(cmd);
        Assert.True(actualResult.HasErrors);
        foreach (var name in cmd.Names)
            Assert.Equal(actualResult.Results[name].Message, AddPartResult.FailureForNameAlreadyInUse.Message);
    }

    [Fact]
    public void FailGivenParentDoesNotExist()
    {
        var sut = _fixture.Create<SkillBlueprint>();
        var names = _fixture.CreateMany<Name>();
        var addPartCmd = new AddPartCmd(_fixture.Create<Name>(), sut.RootPartId);
        sut.TryAdd(addPartCmd, out var partResult);
        var cmd = new AddMultiplePartsCmd(names, parentId: Guid.NewGuid(), sut.RootPartId);

        //Act
        var actualResult = sut.Add(cmd);
        Assert.IsAssignableFrom< AddMultiplePartsResultNoParentNodeFound>(actualResult);
    }

}
