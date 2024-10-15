using Domain.Constants;
using Domain.Enums;

namespace Application.DTOs.Pagination
{
    public class UserParams : PaginationParams
    {
        private int minAge = SystemPolicy.UsersMinimumAge;
        private int? maxAge = null;
        public OrderByOptions OrderBy { get; set; } = OrderByOptions.LastActive;
        public int MinAge
        {
            get
            {
                return minAge;
            }
            set
            {
                if (value >= SystemPolicy.UsersMinimumAge)
                {
                    minAge = value;
                }
            }
        }
        public int? MaxAge
        {
            get
            {
                return maxAge;
            }
            set
            {
                if (value >= minAge)
                {
                    maxAge = value;
                }
            }
        }
    }


}
