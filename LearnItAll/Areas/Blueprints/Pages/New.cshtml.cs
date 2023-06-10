using LearnItAll.Models.Skillblueprints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mtx.LearnItAll.Core.Common.Parts;
using Mtx.LearnItAll.Core.Resources;

namespace LearnItAll.Areas.Blueprints.Pages;

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
	public AddManyPartsModel AddManyPartsModel { get; set; } = new();

	public DeletePartModel DeletePartModel { get; set; } = new();
	public DeleteBlueprintModel DeleteBlueprintModel { get; set; } = new();
	public Guid IdOfLatestAddedPart { get; set; }
	public Guid BlueprintId { get; set; }
	public Guid RootPartId { get; set; }


	public async Task<IActionResult> OnPost()
	{
		var result = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel);
		if (result.IsError)
			return await Task.FromResult(StatusCode(result.StatusCode));

		var data = result.Contents;

		return await this.PartialView("_PartDetail", new RootPartDetail { Part = new Part { Id = data.RootPartId, Name = NewBlueprintModel.Name, ParentId = data.Id }, BlueprintId = data.Id });

	}
	public async Task<IActionResult> OnPostAddMany(CancellationToken ct)
	{

		var result = await mediator.Send(AddManyPartsModel.ToCmd());

		if (!result.HasErrors)
		{
			var details = new AddManyPartsModelResult(AddManyPartsModel.ParentId, AddManyPartsModel.BlueprintId, result);
			return await this.PartialView("_MultiPartDetail", details);
		}
		else
		{
			Response.StatusCode = StatusCodes.Status400BadRequest;
			return await this.PartialView("_ErrorPartial", CoreMessages.Skill_AddingMultipleParts_Failed);
		}

	}

	public async Task<IActionResult> OnPostAdd()
	{
		var result = AddPartResult.FailureForUnknownReason;
		result = await mediator.Send((AddPartCmd)AddPartModel);
		if (result.IsError)
		{
			Response.StatusCode = result.StatusCode;
			return await this.PartialView("_ErrorPartial", result.Message);
		}


		IdOfLatestAddedPart = result.Contents.Value;

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
			});

	}

	public async Task<IActionResult> OnPostDelete()
	{
		var result = DeletePartResult.NoContent204();
		result = await mediator.Send((DeletePartCmd)DeletePartModel);

		if (result.IsSuccess)
			return await Task.FromResult(new OkResult());
		else
		{
			Response.StatusCode = result.StatusCode;
			return await this.PartialView("_ErrorPartial", result.Message);
		}
	}

	public async Task<IActionResult> OnPostDeleteBlueprint()
	{
		var result = DeleteBlueprintResult.CreateInternalError;
		result = await mediator.Send((DeleteBlueprintCmd)DeleteBlueprintModel);

		if (result.Success)
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

