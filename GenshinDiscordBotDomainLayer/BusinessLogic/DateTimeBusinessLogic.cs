using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ResultModels;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class DateTimeBusinessLogic
    {
        private IDateTimeProvider DateTimeProvider { get; set; }

        public bool ParseLocalDateTime(string toParse, CultureInfo culture, out DateTime parsed)
        {
            if (!DateTime.TryParse(
                toParse, culture, DateTimeStyles.AssumeLocal, out DateTime dateTime))
            {
                parsed = DateTimeProvider.GetDateTime();
                return false;
            }
            parsed = dateTime;
            return true;
        }

        public bool ParseTimeSpan(string toParse, out TimeSpan parsed)
        {
            if (!TimeSpan.TryParse(toParse, out TimeSpan timeSpan))
            {
                parsed = TimeSpan.Zero;
                return false;
            }
            parsed = timeSpan;
            return true;
        }

        public DateTimeBusinessLogic(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public ulong GetSecondsInHours(uint hours) => hours * 3600;

        public ulong GetTimeAsUnixSeconds(DateTime dateTime)
        {
            var dateTimeWithoutMillis = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second);
            var diff = dateTimeWithoutMillis - DateTime.UnixEpoch;
            return Convert.ToUInt64(diff.TotalSeconds);
        }

        public ulong GetUtcTimeAsUnixSeconds(DateTime dateTime)
        {
            var dateTimeWithoutMillis = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second);
            var dateTimeWithoutMillisUtc = dateTimeWithoutMillis.ToUniversalTime();
            var diff = dateTimeWithoutMillisUtc - DateTime.UnixEpoch;
            return Convert.ToUInt64(diff.TotalSeconds);
        }

        public DateTime GetCurrentUtcDateTime()
        {
            return DateTimeProvider.GetDateTime().ToUniversalTime();
        }

        public ulong GetCurrentUtcTimeAsUnixSeconds()
        {
            var now = DateTimeProvider.GetDateTime();
            var utcNow = now.ToUniversalTime();
            return GetTimeAsUnixSeconds(utcNow);
        }

        public DateTime GetUtcTimeFromUnixSeconds(ulong unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds);
        }

        public DateTime GetLocalTimeFromUnixSeconds(ulong unixSeconds)
        {
            return GetUtcTimeFromUnixSeconds(unixSeconds).ToLocalTime();
        }

        /// <summary>
        /// Gets the time at which reminder with given duration must be fired.
        /// </summary>
        /// <param name="reminderDuration">Duration of the reminder.</param>
        /// <returns>Time at which reminder must be fird (in UTC) in Unix seconds (number of seconds from January 1st, 1970 00:00:00).</returns>
        public ulong GetReminderUtcTimeAsUnixSeconds(TimeSpan reminderDuration)
        {
            TimeSpan durationWithoutMillis = 
                new(reminderDuration.Days, reminderDuration.Hours,
                reminderDuration.Minutes, reminderDuration.Seconds, 0);
            ulong durationInSeconds = Convert.ToUInt64(durationWithoutMillis.TotalSeconds);
            ulong currentUtcTimeAsUnixSeconds = GetCurrentUtcTimeAsUnixSeconds();
            return currentUtcTimeAsUnixSeconds + durationInSeconds;
        }

        /// <summary>
        /// Gets next time (in Unix seconds) at which reminder that must be fired at a given local time must be fired.
        /// </summary>
        /// <param name="time">Time at which reminder must be fired. (Local)</param>
        /// <returns>Time at which reminder must be fird (in UTC) in Unix seconds (number of seconds from January 1st, 1970 00:00:00).</returns>
        public ulong GetUtcTimeToNextDailyReminderAsUnixSeconds(TimeOnly time)
        {
            // Note that duration returned by GetDurationToNextDailyReminder (which uses local time) 
            // is time zone-agnostic. 
            return GetReminderUtcTimeAsUnixSeconds(GetDurationToNextDailyReminder(time));
        }

        // gets next DateTime when user should be reminded at this time
        // for example: if time = 18:00 and current date time is 1/1/2000 21:00
        // next reminder datetime will be 2/1/2000 18:00
        /// <summary>
        /// Gets the next date and time at which reminder with a given time must be fired.
        /// </summary>
        /// <param name="time">Time at which reminder must be fired. (Local)</param>
        /// <returns>Local date and time at which reminder must be fired.</returns>
        private DateTime GetNextDailyReminderTimeFor(TimeOnly time)
        {
            DateTime dtNow = DateTime.Now;
            DateTime nextReminderDateTime 
                = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, time.Hour, time.Minute, 0, DateTimeKind.Utc);
            if (nextReminderDateTime < dtNow)
            {
                nextReminderDateTime = nextReminderDateTime.AddDays(1);
            }
            return nextReminderDateTime;
        }

        /// <summary>
        /// Gets reminder duration for a reminder that must be fired at a given local time.
        /// </summary>
        /// <param name="time">Time at which reminder must be fired. (Local)</param>
        /// <returns>Duration of the reminder.</returns>
        private TimeSpan GetDurationToNextDailyReminder(TimeOnly time)
        {
            var dtOfNextReminder = GetNextDailyReminderTimeFor(time);
            return dtOfNextReminder - DateTime.Now;
        }
    }
}
