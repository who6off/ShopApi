namespace HelloApi.Extensions
{
    public static class DateTimeExtension
    {
        public static int GetYearDifference(this DateTime a, DateTime b)
        {
            var difference = a.Year - b.Year;

            if ((a.Month < b.Month) || (a.Month == b.Month && a.Day < b.Day))
                difference--;

            return difference;
        }

        public static int GetYearDifference(this DateTime a, string b)
        {
            return a.GetYearDifference(DateTime.Parse(b));
        }
    }
}
