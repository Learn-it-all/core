using LearnItAll.Models.Skillblueprints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading.Tasks;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [BindProperties]
    public class CreateBlueprintModel : PageModel
    {
        private readonly IMediator mediator;

        public CreateBlueprintModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public SkillBluePrint Blueprint { get; set; } = NullSkillBluePrint.New();
        public NewBlueprintModel NewBlueprintModel { get; set; } = new();
        public AddPartModel AddPartModel { get; set; } = new();

        public DeletePartModel DeletePartModel { get; set; } = new();
        public Guid IdOfLatestAddedPart { get; set; }
        public Guid BlueprintId { get; set; }
        public Guid RootPartId { get; set; }
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
            var result = AddPartResult.FailureForUnknownReason;
            //Note: any exception thrown by the mediator handler will bubble up and endup in http 500;
            //to handle exceptions there a exception handler is required to be implemented 
            //see https://github.com/jbogard/MediatR/tree/master/samples/MediatR.Examples/ExceptionHandler
            //and https://github.com/jbogard/MediatR/wiki#exceptions-handling
            result = await mediator.Send((AddPartCmd)AddPartModel);
            IdOfLatestAddedPart = result.IdOfAddedPart;

            if (result.IsSuccess)
                return await this.PartialView("_PartDetail",
                    new PartDetail
                    {
                        Part = new Part
                        {
                            Id = IdOfLatestAddedPart,
                            Name = AddPartModel.Name,
                            ParentId = AddPartModel.ParentId,
                        },
                        BlueprintId = AddPartModel.BlueprintId,
                        IdentationLevel = IdentationLevel + 1
                    });
            else
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return await this.PartialView("_ErrorPartial", result.Message);
            }

        }

        public async Task<IActionResult> OnPostDelete()
        {
            var result = DeletePartResult.FailureForPartNotFound;
            result = await mediator.Send((DeletePartCmd)DeletePartModel);

            if (result.IsSuccess)
                return await Task.FromResult(new OkResult());
            else
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return await this.PartialView("_ErrorPartial", result.Message);
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

