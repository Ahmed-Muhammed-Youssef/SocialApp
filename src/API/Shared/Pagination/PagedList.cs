namespace Shared.Pagination;

public class PagedList<T>
{
    public List<T> Items { get; private set; }
    public PagedList(List<T> items, int count, int pageNumber, int itemsPerPage)
    {
        CurrentPage = pageNumber; // note that: it begins with 1 
        ItemsPerPage = itemsPerPage;
        TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage);
        Count = count;
        if (items is not null)
        {
            Items = items;
        }
        else
        {
            Items = [];
        }
    }

    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int ItemsPerPage { get; private set; }
    public int Count { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
