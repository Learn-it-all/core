using LearnItAll.Models.Skillblueprints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [AllowAnonymous]
    [BindProperties]
    public class BlueprintModel : PageModel
    {
        private readonly IMediator mediator;

        public BlueprintModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public SkillBluePrint Blueprint { get; set; } = NullSkillBluePrint.New();
        [BindProperty]
        public NewBlueprintModel NewBlueprintModel { get; set; } = new();
        [BindProperty]
        public AddPartModel AddPartModel { get; set; } = new();
        public Guid IdOfLatestAddedPart { get; set; }
        public Guid BlueprintId { get; set; }
        public string BlueprintName { get; private set; }

        public async Task OnGet()
        {
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostCreate()
        {
            var id = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);
            string json = this.HttpContext.Session.GetString(id.ToString()) ?? throw new InvalidOperationException("SkillBlueprint not found on Session");
            Blueprint = JsonConvert.DeserializeObject<SkillBluePrint>(json);
            return await Task.FromResult(Page());
            
        }
        public async Task<IActionResult> OnPostCreateSimple()
        {
            var id = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);
            BlueprintId = id;
            BlueprintName = NewBlueprintModel.Name;
            return await Task.FromResult(Page());

        }

        public async Task<IActionResult> OnPostAdd()
        {
            try
            {
                IdOfLatestAddedPart = await mediator.Send((AddPartCmd)AddPartModel);

            }
            catch (Exception e ) 
            {
                string blueprint = this.HttpContext.Session.GetString(AddPartModel.SkillId.ToString()) ?? throw new InvalidOperationException("SkillBlueprint not found on Session");
                Blueprint = JsonConvert.DeserializeObject<SkillBluePrint>(blueprint);
                AddPartModel = new();
                ModelState.Clear();
                ModelState.AddModelError($"{nameof(AddPartModel)}.{nameof(AddPartModel.Name)}", e.Message);
                return await Task.FromResult(Page());
            }
            string json = this.HttpContext.Session.GetString(AddPartModel.SkillId.ToString()) ?? throw new InvalidOperationException("SkillBlueprint not found on Session");
            Blueprint = JsonConvert.DeserializeObject<SkillBluePrint>(json);
            AddPartModel = new();
            return await Task.FromResult(Page());
        }

    }
    public class AddPartModel
    {
        [Required]
        [MaxLength(Mtx.LearnItAll.Core.Common.Name.MaxLenght)]
        public string Name { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public Guid SkillId { get; set; }

        public static implicit operator AddPartCmd(AddPartModel model) => new AddPartCmd(new Name(model.Name), model.ParentId,model.SkillId);
    }

    public class NewBlueprintModel
    {
        [Required]
        [MaxLength(Mtx.LearnItAll.Core.Common.Name.MaxLenght)]
        public string Name { get; set; } = string.Empty;

        public static implicit operator CreateSkillBlueprintCmd(NewBlueprintModel model) => new CreateSkillBlueprintCmd(new Name(model.Name));
    }
}

