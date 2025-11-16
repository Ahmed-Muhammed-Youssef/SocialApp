namespace Shared.Pagination;

public record PaginationParams
{
    public int PageNumber { get; init; }
    public int ItemsPerPage { get; init; }
    public int SkipValue() => (PageNumber - 1) * ItemsPerPage;
}
