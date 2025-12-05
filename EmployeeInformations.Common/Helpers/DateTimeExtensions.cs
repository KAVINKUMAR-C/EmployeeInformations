using System.Globalization;

namespace EmployeeInformations.Common.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToDatetime(string dateValue)
        {
            return DateTime.ParseExact(dateValue, Constant.DateFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertToNotNullDatetimeTimeSheet(string dateValue)
        {
            return DateTime.ParseExact(dateValue, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertToNotNullDatetime(string dateValue)
        {
            return DateTime.ParseExact(dateValue, Constant.DateFormat, CultureInfo.InvariantCulture);
        }
    }
}
