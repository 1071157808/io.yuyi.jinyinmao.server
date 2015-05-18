// FileInformation: nyanya/Infrastructure.Lib/DatetimeExtensions.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/15   4:12 PM

using System;

namespace Infrastructure.Lib.Extensions
{
    public static class DatetimeExtensions
    {
        public static string ToMeowFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string ToMeowFormat(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
        }

        public static string ToTimestamp(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmssffff");
        }

        public static string ToShortFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}