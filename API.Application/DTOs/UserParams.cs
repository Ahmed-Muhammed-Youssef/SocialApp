namespace API.Application.DTOs
{
    public class UserParams : PaginationParams
    {

        private int? minAge = null;
        private int? maxAge = null;
        private string orderBy = "lastActive";

        public string Sex { get; set; } // to get the default value we need to make a query which is the interest field. 
        public int? MinAge
        {
            get
            {
                return minAge;
            }
            set
            {
                if (value >= 18)
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
                if (minAge != null)
                {
                    if (value >= minAge)
                    {
                        maxAge = value;
                    }
                }
                else if (value >= 18)
                {
                    maxAge = value;
                }
            }
        }
        private List<string> orderOptions = new List<string>() { "lastActive", "age", "creationTime" };
        public string OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) || orderOptions.Contains(value))
                {
                    orderBy = value;
                }
            }
        }
    }
}
