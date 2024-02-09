using API.Domain.Constants;
using API.Domain.Enums;

namespace API.Application.DTOs
{
    public class UserParams : PaginationParams
    {
        private int minAge = SystemPolicy.UsersMinimumAge;
        private int? maxAge = null;
        public SexOptions Sex { get; set; } = SexOptions.None; // to get the default value we need to make a query which is the interest field. 
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
