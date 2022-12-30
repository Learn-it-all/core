using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearnItAll.Models.Skillblueprints;
public class SkillBluePrint
{
    public Guid Id { get; set; }

    public virtual bool IsNull => false;

    public string Name => _root.Name;

    [JsonIgnore]
    public IReadOnlyCollection<PartNode> Nodes => _root.Nodes;

    [JsonIgnore]
    public IReadOnlyCollection<Part> Parts => _root.Parts;

    [JsonIgnore]
    public Guid SkillId => _root.Id;

    [JsonProperty]
    private readonly PartNode _root = new NullPartNode();

#pragma warning disable CS8618
    public SkillBluePrint()
    {

    }
#pragma warning restore CS8618

}

public class NullSkillBluePrint : SkillBluePrint
{
    public override bool IsNull => true;
    internal static SkillBluePrint New()
    {
        return new NullSkillBluePrint();
    }

}
public class NullPartNode : PartNode
{
    public override bool IsNull => true;
}
public class AddPartModel
{
    [Required]
    [MaxLength(Mtx.LearnItAll.Core.Common.Name.MaxLenght)]
    public string Name { get; set; } = string.Empty;
    public Guid ParentId { get; set; }
    public Guid BlueprintId { get; set; }

    public static implicit operator AddPartCmd(AddPartModel model) => new AddPartCmd(new Name(model.Name), model.ParentId, model.BlueprintId);
}

public class AddManyPartsModel : IModelValidator
{
    [Required]
    public List<string> Names { get; set; } = new List<string>();
    public Guid ParentId { get; set; }
    public Guid BlueprintId { get; set; }

    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        return new List<ModelValidationResult>();
    }

 }
public class DeletePartModel
{
    public Guid PartId { get; set; }
    public Guid BlueprintId { get; set; }

    public static implicit operator DeletePartCmd(DeletePartModel model) => new DeletePartCmd(partId: model.PartId, blueprintId: model.BlueprintId);
}
public class DeleteBlueprintModel
{
    public Guid BlueprintId { get; set; }

    public static implicit operator DeleteBlueprintCmd(DeleteBlueprintModel model) => new DeleteBlueprintCmd(blueprintId: model.BlueprintId);
}

public class NewBlueprintModel
{
    [Required]
    [MaxLength(Mtx.LearnItAll.Core.Common.Name.MaxLenght)]
    public string Name { get; set; } = string.Empty;

    public static implicit operator CreateSkillBlueprintCmd(NewBlueprintModel model) => new CreateSkillBlueprintCmd(new Name(model.Name));
}