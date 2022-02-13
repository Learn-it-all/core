namespace Mtx.LearnItAll.Core.Common.Parts;

public record DeleteBlueprintResultInternalError : DeleteBlueprintResult
{
    public override string Message => $"Operation failed";
    public override int Code => 3;
}