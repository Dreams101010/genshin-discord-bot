using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class DateTimeBusinessLogic
    {
        private IDateTimeProvider DateTimeProvider { get; set; }

        public DateTimeBusinessLogic(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public ulong GetReminderTimeAsUnixSeconds(TimeSpan duration)
        {
            TimeSpan durationWithoutMilliseconds = 
                new(duration.Days, duration.Hours, duration.Minutes, duration.Seconds, 0);
            ulong durationInSeconds = Convert.ToUInt64(duration.TotalSeconds);
            ulong currentTimeAsUnixSeconds = GetCurrentUtcTimeAsUnixSeconds();
            return currentTimeAsUnixSeconds + durationInSeconds;
        }

        public ulong GetCurrentUtcTimeAsUnixSeconds()
        {
            var now = DateTimeProvider.GetDateTime();
            var utcNow = now.ToUniversalTime();
            var utcNowWithoutMilliseconds = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
            var diff = utcNowWithoutMilliseconds - DateTime.UnixEpoch;
            return Convert.ToUInt64(diff.TotalSeconds);
        }

        public ulong GetHoursAsTotalSeconds(uint hours)
        {
            return hours * 60 * 60;
        }

        public DateTime GetUtcTimeFromUnixSeconds(ulong unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds);
        }

        public DateTime GetLocalTimeFromUnixSeconds(ulong unixSeconds)
        {
            return GetUtcTimeFromUnixSeconds(unixSeconds).ToLocalTime();
        }
    }
}
