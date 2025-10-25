namespace Application.DTOs.Pagination;

public class PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
{
    public int CurrentPage { get; private set; } = currentPage;
    public int ItemsPerPage { get; private set; } = itemsPerPage;
    public int TotalItems { get; private set; } = totalItems;
    public int TotalPages { get; private set; } = totalPages;
}
