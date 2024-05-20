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
            string format = "M/d/yyyy h:mm:ss tt";

            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            {
                return d;
            }
            return null;
        }
    }
}