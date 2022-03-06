namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 40;
        private int pageNumber = 1;
        public int PageNumber { 
            get {
                return pageNumber; 
            }
            set {
                if(value > 0)
                {
                    pageNumber = value;
                }
            } 
        }
        private int itemsPerPage;
        public int ItemsPerPage { 
            get {
                return itemsPerPage;
            }
            set { 
                if(value > MaxPageSize || value < 1)
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
