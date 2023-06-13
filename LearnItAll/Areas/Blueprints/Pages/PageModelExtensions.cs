using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnItAll.Areas.Blueprints.Pages;

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

