using System;

namespace Infrastructure.Helpers
{
    public static class DateTimeHelpers
    {
        public static DateTime FromUTCToLocal(this DateTime d, string timezoneId = "India Standard Time")
        {
            TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(d, info);
        }
    }
}
