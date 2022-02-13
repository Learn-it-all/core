namespace Mtx.LearnItAll.Core.Common.Parts;

public record DeleteBlueprintResultSuccess : DeleteBlueprintResult
{
    public override string Message => $"Operation suceeded";
    public override int Code => 1;
    public override bool Success => true;
}
