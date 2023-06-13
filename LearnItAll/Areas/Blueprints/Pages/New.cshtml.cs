using LearnItAll.Models.Skillblueprints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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


	public async Task<IActionResult> OnPost(CancellationToken ct)
	{
		var result = await mediator.Send((CreateSkillBlueprintCmd)NewBlueprintModel, ct);
		if (result.IsError)
			return await Task.FromResult(StatusCode(result.StatusCode));

		var data = result.Contents;

		return await this.PartialView("_PartDetail", new RootPartDetail { Part = new Part { Id = data.RootPartId, Name = NewBlueprintModel.Name, ParentId = data.Id }, BlueprintId = data.Id });

	}
	public async Task<IActionResult> OnPostAddMany(CancellationToken ct)
	{

		var result = await mediator.Send(AddManyPartsModel.ToCmd(),ct);

		if (!result.HasErrors)
		{
			var details = new AddManyPartsModelResult(AddManyPartsModel.ParentId, AddManyPartsModel.BlueprintId, result);
			return await this.PartialView("_MultiPartDetail", details);
		}
		else
		{
			Response.StatusCode = result.StatusCode;
			return await this.PartialView("_ErrorPartial", CoreMessages.Skill_AddingMultipleParts_Failed);
		}

	}

	public async Task<IActionResult> OnPostAdd(CancellationToken ct)
	{
		var result = AddPartResult.FailureForUnknownReason;
		result = await mediator.Send((AddPartCmd)AddPartModel, ct);
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

	public async Task<IActionResult> OnPostDelete(CancellationToken ct)
	{
		var result = DeletePartResult.NoContent204();
		result = await mediator.Send((DeletePartCmd)DeletePartModel, ct);

		if (result.IsSuccess)
			return await Task.FromResult(new OkResult());
		else
		{
			Response.StatusCode = result.StatusCode;
			return await this.PartialView("_ErrorPartial", result.Message);
		}
	}

	public async Task<IActionResult> OnPostDeleteBlueprint(CancellationToken ct)
	{
		var result = await mediator.Send((DeleteBlueprintCmd)DeleteBlueprintModel, ct);

		if (result.IsSuccess)
			return await Task.FromResult(new OkResult());
		else
		{
			Response.StatusCode = result.StatusCode;
			return await this.PartialView("_ErrorPartial", result.Message);
		}
	}
}

