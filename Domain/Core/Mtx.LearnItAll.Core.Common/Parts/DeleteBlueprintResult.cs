namespace Mtx.LearnItAll.Core.Common.Parts;

public abstract record DeleteBlueprintResult
{
    public abstract string Message { get; }
    public abstract int Code { get; }
    public virtual bool Success { get; } = false;
    public static DeleteBlueprintResult CreateSuccess => new DeleteBlueprintResultSuccess();
    public static DeleteBlueprintResult CreateInternalError => new DeleteBlueprintResultInternalError();
}
