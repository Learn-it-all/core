using Mtx.Common.Resources;
using Mtx.LearnItAll.Core.Resources;
using Mtx.Results;
using System;

namespace Mtx.LearnItAll.Core.Common.Parts
{
	public record AddPartResult : DataResult<Guid?>
	{
		protected AddPartResult(int StatusCode, Guid? Contents = default, string Message = "", Exception Exception = null) : base(StatusCode, Contents, Message, Exception)
		{
		}

		public static AddPartResult Success(Guid idOfAddedPart) => new AddPartResult(StatusCodes.Status200OK, idOfAddedPart, CommonMessages.Success);

		public static AddPartResult FailureForNameAlreadyInUse => new AddPartResult(StatusCodes.Status400BadRequest, Message: CoreMessages.AddPartResult_DuplicateSibling);

		public static AddPartResult FailureForUnknownReason => new AddPartResult(StatusCodes.Status500InternalServerError, Message: CommonMessages.Error);
		public static AddPartResult FailureForParentNodeNotFound => new AddPartResult(StatusCodes.Status404NotFound, Message: CoreMessages.Skill_ParentIdDoesNotExist);
		public static AddPartResult FromResult(Result source)
		{
			return new AddPartResult(source.StatusCode, null, Message: source.Message, Exception: source.Exception);
		}
	}
}
