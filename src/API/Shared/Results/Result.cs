using System.Diagnostics.CodeAnalysis;

namespace Shared.Results;

public class Result<T> : IResult
{
    public Result(T value) => Value = value;

    protected internal Result(T value, string successMessage) : this(value) => SuccessMessage = successMessage;

    protected Result(ResultStatus status) => Status = status;

    public T? Value { get; init; }

    public ResultStatus Status { get; protected set; } = ResultStatus.Ok;

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Created;

    public string SuccessMessage { get; protected set; } = string.Empty;
    public string CorrelationId { get; protected set; } = string.Empty;
    public string Location { get; protected set; } = string.Empty;
    public IEnumerable<string> Errors { get; protected set; } = [];

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Success(T value, string successMessage) => new(value, successMessage);

    public static Result<T> Created(T value) => new(ResultStatus.Created) { Value = value };

    public static Result<T> Created(T value, string location) => new(ResultStatus.Created) { Value = value, Location = location };

    public static Result<T> Error(string errorMessage) => new(ResultStatus.Error) { Errors = [errorMessage] };

    public static Result<T> NotFound() => new(ResultStatus.NotFound);

    public static Result<T> NotFound(params string[] errorMessages) => new(ResultStatus.NotFound) { Errors = errorMessages };

    public static Result<T> Forbidden() => new(ResultStatus.Forbidden);

    public static Result<T> Forbidden(params string[] errorMessages) => new(ResultStatus.Forbidden) { Errors = errorMessages };

    public static Result<T> Unauthorized() => new(ResultStatus.Unauthorized);

    public static Result<T> Unauthorized(params string[] errorMessages) => new(ResultStatus.Unauthorized) { Errors = errorMessages };

    public static Result<T> Conflict() => new(ResultStatus.Conflict);

    public static Result<T> Conflict(params string[] errorMessages) => new(ResultStatus.Conflict) { Errors = errorMessages };

    public static Result<T> CriticalError(params string[] errorMessages) => new(ResultStatus.CriticalError) { Errors = errorMessages };

    public static Result<T> Unavailable(params string[] errorMessages) => new(ResultStatus.Unavailable) { Errors = errorMessages };

    public static Result<T> NoContent() => new(ResultStatus.NoContent);
}
