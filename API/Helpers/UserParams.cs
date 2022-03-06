namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 40;
        private int pageNumber = 1;
        private int itemsPerPage;
        private int? minAge = null;
        private int? maxAge = null;
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
        public string Sex { get; set; } // to get the default value we need to make a query which is the interest field. 
        public int? MinAge
        {
            get
            {
                return minAge;
            }
            set
            {
                if(value >= 18)
                {
                    minAge = value;
                }
            }
        }
        public int? MaxAge { 
            get { 
                return maxAge; 
            }
            set {
                if(minAge != null)
                {
                    if(value >= minAge)
                    {
                        maxAge = value;
                    }
                }
                else if (value >= 18)
                {
                    maxAge=value;
                }
            }
        }
    }
}
