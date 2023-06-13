namespace Mtx.LearnItAll.Core.Common.Parts;
public record DeletePartResult : Result
{
	protected DeletePartResult(int StatusCode, string Message = "", Exception Exception = null) : base(StatusCode, Message, Exception)
	{
	}

	public static new DeletePartResult NoContent204() => new DeletePartResult(Status204NoContent);
	public static new DeletePartResult NotFound404() => new DeletePartResult(Status404NotFound);
	public static DeletePartResult InternalError() => new DeletePartResult(Status500InternalServerError);
	public static DeletePartResult FromResult(Result original) => new DeletePartResult(original);

}
