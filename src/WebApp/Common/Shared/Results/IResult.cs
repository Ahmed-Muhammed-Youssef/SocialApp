namespace Shared.Results;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    object? GetValue();
    string Location { get; }
}
