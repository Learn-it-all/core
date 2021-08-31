using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [AllowAnonymous]
    [BindProperties]
    public class SkillsModel : PageModel
    {
        private readonly IMediator mediator;

        public SkillsModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public SkillBluePrint Skill { get; set; } = new();
        public NewBlueprintModel NewBlueprintModel { get; set; } = new();

        public async Task OnGet()
        {
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPost()
        {
            await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);
            return await Task.FromResult(new ContentResult());
        }

    }

    public class NewBlueprintModel
    {
        [Required]
        [MaxLength(Mtx.LearnItAll.Core.Common.Name.MaxLenght)]
        public string Name { get; set; } = string.Empty;

        public static implicit operator CreateSkillBlueprintCmd(NewBlueprintModel model) => new CreateSkillBlueprintCmd(new Name(model.Name));
    }

    public class SkillBluePrint
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get => Root.Name; }
        public List<Part> Parts { get; set; } = new();
        public PartNode Root { get; set; } = new();

    }

    public class Part
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class PartNode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public List<Part> Parts { get; set; } = new();
        public List<PartNode> Nodes { get; set; } = new();
    }
}