using Mtx.Common.Resources;
using Mtx.Results;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
	public record CreateSkillBlueprintResult : DataResult<SkillBlueprintData>
	{
		protected CreateSkillBlueprintResult(int StatusCode, SkillBlueprintData? Contents = default, string Message = "", Exception Exception = null) : base(StatusCode, Contents, Message, Exception)
		{
		}

		public static CreateSkillBlueprintResult FromSkillData(SkillBlueprintData data) => new CreateSkillBlueprintResult(StatusCodes.Status200OK, data, CommonMessages.Success);

		public static CreateSkillBlueprintResult FromResult(Result source, SkillBlueprintData data = default)
		{
			return new CreateSkillBlueprintResult(source.StatusCode, data, Message: source.Message, Exception: source.Exception);
		}
	}
}
