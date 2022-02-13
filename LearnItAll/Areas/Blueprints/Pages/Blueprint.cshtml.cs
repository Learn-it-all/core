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
        public string BlueprintName { get; private set; } = string.Empty;

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
            var blueprintdata = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);
            BlueprintId = blueprintdata.Id;
            BlueprintName = NewBlueprintModel.Name;
            return await Task.FromResult(Page());

        }

        public async Task<IActionResult> OnPostAdd()
        {
            var result = AddPartResult.FailureForUnknownReason;
            try
            {
                result = await mediator.Send((AddPartCmd)AddPartModel);
                IdOfLatestAddedPart = result.IdOfAddedPart;
            }
            catch (Exception e ) 
            {
                string blueprint = this.HttpContext.Session.GetString(AddPartModel.BlueprintId.ToString()) ?? throw new InvalidOperationException("SkillBlueprint not found on Session");
                Blueprint = JsonConvert.DeserializeObject<SkillBluePrint>(blueprint);
                AddPartModel = new();
                ModelState.Clear();
                ModelState.AddModelError($"{nameof(AddPartModel)}.{nameof(AddPartModel.Name)}", e.Message);
                return await Task.FromResult(Page());
            }
            string json = this.HttpContext.Session.GetString(AddPartModel.BlueprintId.ToString()) ?? throw new InvalidOperationException("SkillBlueprint not found on Session");
            Blueprint = JsonConvert.DeserializeObject<SkillBluePrint>(json);
            AddPartModel = new();
            return await Task.FromResult(Page());
        }

    }
  
}

