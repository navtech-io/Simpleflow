// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System;

namespace Simpleflow.Functions
{
    internal static class DateTimeFunctions
    {
        public static DateTime Date(int y, int m, int d, int h, int mn, int s) => new DateTime(y,m,d,h,mn,s);

        public static DateTime GetCurrentDate() => DateTime.Now.Date;
        public static DateTime GetNow(string timeZone)
        {
            return timeZone == default(string) ?
                   DateTime.Now :  
                   TimeZoneInfo.ConvertTimeFromUtc(
                        DateTime.UtcNow, 
                        TimeZoneInfo.FindSystemTimeZoneById(timeZone));
        }
        public static TimeSpan GetCurrentTime() => DateTime.Now.TimeOfDay;
    }
}
