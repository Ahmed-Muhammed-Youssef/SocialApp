namespace Notifications.API.Contracts;

/// <summary>Cursor-free page wrapper returned by list endpoints.</summary>
public sealed record PagedResponse<T>
{
    public required List<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public bool HasNextPage => Page * PageSize < TotalCount;
}
