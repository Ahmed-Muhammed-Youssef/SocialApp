using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Pagination
{

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
                Items = new List<T>();
            }
            // AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int Count { get; set; } // total number of items
    }
}
