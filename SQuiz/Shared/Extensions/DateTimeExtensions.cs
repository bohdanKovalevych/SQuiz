namespace SQuiz.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static string? ToShortDateString(this DateTimeOffset? date)
        {
            return date?.ToString("MM/dd");
        }

        public static string? ToShortTimeString(this DateTimeOffset? date)
        {
            return date?.ToString("HH:mm");
        }
    }
}
