namespace Shared.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Calculates the age in years based on the specified date of birth.
    /// </summary>
    /// <param name="dob">The date of birth from which to calculate the age.</param>
    /// <returns>The age in years. Returns 0 if the calculated age is negative.</returns>
    public static int CalculateAge(this DateTime dob)
    {
        DateTime now = DateTime.UtcNow;
        int age = now.Year - dob.Year;
        // Adjust if birthday hasn't occurred yet
        if (dob > now.AddYears(-age))
        {
            age--;
        }
        return age > 0 ? age : 0;
    }

    /// <summary>
    /// Converts the specified <see cref="DateTime"/> to a human-readable relative time string.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert to a relative time string.</param>
    /// <returns>A string representing the relative time from the specified <paramref name="dateTime"/> to the current UTC time.
    /// Returns "just now" if the time difference is less than one minute, or a formatted string indicating the number
    /// of minutes, hours, or days ago. If the date is more than a week ago, returns the date formatted as "dd MMM
    /// yyyy".</returns>
    public static string ToRelativeTimeString(this DateTime dateTime)
    {
        var timeDiff = DateTime.UtcNow - dateTime;

        if (timeDiff.TotalMinutes < 1)
        {
            return "just now";
        }

        if (timeDiff.TotalHours < 1)
        {
            return $"{(int)timeDiff.TotalMinutes}m ago";
        }

        if (timeDiff.TotalDays < 1)
        {
            return $"{(int)timeDiff.TotalHours}h ago";
        }

        if (timeDiff.TotalDays < 7)
        {
            return $"{(int)timeDiff.TotalDays}d ago";
        }

        return dateTime.ToString("dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }
}
