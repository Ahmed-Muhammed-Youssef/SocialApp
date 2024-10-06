﻿namespace Application.DTOs.Pagination
{
    public class PaginationParams
    {
        private const int MaxPageSize = 40;
        private int pageNumber = 1;
        private int itemsPerPage = 2;
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
                if (value > MaxPageSize || value < 1)
                {
                    itemsPerPage = MaxPageSize;
                }
                else
                {
                    itemsPerPage = value;
                }
            }
        }
    }
}
