namespace Shared.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateTime dob)
    {
        DateTime now = DateTime.Now;
        int age = now.Year - dob.Year;
        // Adjust if birthday hasn't occurred yet
        if (dob > now.AddYears(-age))
        {
            age--;
        }
        return age;
    }
    public static string ToRelativeTimeString(this DateTime dateTime)
    {
        var timeDiff = DateTime.Now - dateTime;

        if (timeDiff.TotalMinutes < 1)
            return "just now";
        if (timeDiff.TotalHours < 1)
            return $"{(int)timeDiff.TotalMinutes}m ago";
        if (timeDiff.TotalDays < 1)
            return $"{(int)timeDiff.TotalHours}h ago";
        if (timeDiff.TotalDays < 7)
            return $"{(int)timeDiff.TotalDays}d ago";

        return dateTime.ToString("dd MMM yyyy");
    }
}
