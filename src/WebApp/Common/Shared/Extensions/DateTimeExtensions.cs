namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
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
}
