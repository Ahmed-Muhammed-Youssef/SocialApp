using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PagedList<T>: List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int itemsPerPage)
        {
            CurrentPage = pageNumber; // note that: it begins with 1 
            ItemsPerPage = itemsPerPage;
            TotalPages = (int) Math.Ceiling(count/(double) itemsPerPage);
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } 
        public int TotalCount { get; set; } // total number of items

        public static async Task<PagedList<T>> CreatePageAsync(IQueryable<T> source, int pageNumber, int itemsPerPage)
        {
            int count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, itemsPerPage);
        }

    }
}
