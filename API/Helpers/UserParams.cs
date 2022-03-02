namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize { 
            get {
                return pageSize;
            }
            set { 
                if(value > MaxPageSize)
                {
                    PageSize = MaxPageSize;
                }
                else
                {
                    PageSize = value;
                }
            } 
        }
    }
}
