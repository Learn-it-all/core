using LearnItAll.Models.Skillblueprints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading.Tasks;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [AllowAnonymous]
    [BindProperties]
    public class CreateBlueprintModel : PageModel
    {
        private readonly IMediator mediator;

        public CreateBlueprintModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public SkillBluePrint Blueprint { get; set; } = NullSkillBluePrint.New();
        [BindProperty]
        public NewBlueprintModel NewBlueprintModel { get; set; } = new();
        [BindProperty]
        public AddPartModel AddPartModel { get; set; } = new();
        public Guid IdOfLatestAddedPart { get; set; }
        [BindProperty]
        public Guid BlueprintId { get; set; }
        [BindProperty]
        public Guid RootPartId { get; set; }
        [BindProperty]
        public int IdentationLevel { get; set; }
        public async Task OnGet()
        {
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostCreate()
        {
            var blueprintData = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);

            return await this.PartialView("_PartDetail", new RootPartDetail { Part = new Part { Id = blueprintData.RootPartId, Name = NewBlueprintModel.Name, ParentId = blueprintData.Id }, BlueprintId = blueprintData.Id });

        }

        public async Task<IActionResult> OnPostAdd()
        {
            try
            {
                IdOfLatestAddedPart = await mediator.Send((AddPartCmd)AddPartModel);
                return await this.PartialView("_PartDetail",
                    new PartDetail { 
                        Part = new Part { 
                                Id = IdOfLatestAddedPart, 
                                Name = AddPartModel.Name, 
                                ParentId = AddPartModel.ParentId,
                                },
                        BlueprintId = AddPartModel.BlueprintId,
                        IdentationLevel = IdentationLevel + 1});
            }
            catch (Exception e)
            {
                //TODO: render the error using HTMX
                return await Task.FromResult(new BadRequestObjectResult(e));
                //return await this.PartialView("_ErrorPartial", e.Message);
            }

        }

    }
    public static class PageModelExtensions
    {
        public static Task<PartialViewResult> PartialView(this PageModel page, string name, object model)
        {
            var viewDataDictionary = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            viewDataDictionary.Model = model;
            var view = new PartialViewResult()
            {
                ViewName = name,
                ViewData = viewDataDictionary,
                TempData = page.TempData
            };

            return Task.FromResult(view);
        }
    }
}

