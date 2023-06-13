namespace Mtx.LearnItAll.Core.Common.Parts;

public record DeleteBlueprintResult : Result
{
	public DeleteBlueprintResult(int StatusCode, string Message = "", Exception Exception = null) : base(StatusCode, Message, Exception)
	{
	}
	public static new DeleteBlueprintResult NoContent204() => new DeleteBlueprintResult(Status204NoContent);
	public static new DeleteBlueprintResult NotFound404() => new DeleteBlueprintResult(Status404NotFound);
	public static DeleteBlueprintResult InternalError() => new DeleteBlueprintResult(Status500InternalServerError);
	public static DeleteBlueprintResult FromResult(Result original) => new DeleteBlueprintResult(original);

}
