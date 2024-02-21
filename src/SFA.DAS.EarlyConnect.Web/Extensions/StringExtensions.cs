using System.Globalization;

namespace SFA.DAS.EarlyConnect.Web.Extensions
{
    public static class StringExtensions
    {
        private static readonly IFormatProvider _ukCulture = new CultureInfo("en-GB");

        public static DateTime? AsUKDate(this string date)
        {
            if (DateTime.TryParseExact(date, "d/M/yyyy", _ukCulture, DateTimeStyles.AssumeUniversal, out var d))
            {
                return d;
            }

            return null;
        }

        public static DateTime? AsUKDateTime(this string date)
        {
            if (DateTime.TryParse(date, _ukCulture, out var d))
            {
                return d;
            }
            return null;
        }
    }
}