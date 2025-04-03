using Domain.Constants;

namespace Application.DTOs.Pagination
{
    public class PaginationParams
    {
        private int pageNumber = 1;
        private int itemsPerPage = 10;
        public int SkipValue { 
            get {
                return (PageNumber - 1) * ItemsPerPage;
            } 
        }
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                if (value > 0)
                {
                    pageNumber = value;
                }
            }
        }
        public int ItemsPerPage
        {
            get
            {
                return itemsPerPage;
            }
            set
            {
                if (value > SystemPolicy.MaxPageSize || value < 1)
                {
                    itemsPerPage = SystemPolicy.MaxPageSize;
                }
                else
                {
                    itemsPerPage = value;
                }
            }
        }
    }
}
