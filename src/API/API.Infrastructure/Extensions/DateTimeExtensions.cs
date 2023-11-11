namespace API.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dob.Year;
            // we need this for leap years 
            if (dob > now.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
