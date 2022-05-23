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

        public ulong GetCurrentUtcTimeAsUnixSeconds()
        {
            var now = DateTimeProvider.GetDateTime();
            var utcNow = now.ToUniversalTime();
            var utcNowWithoutMilliseconds = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
            var diff = utcNowWithoutMilliseconds - DateTime.UnixEpoch;
            Console.WriteLine(diff.TotalSeconds);
            Console.WriteLine(Convert.ToUInt64(diff.TotalSeconds));
            return Convert.ToUInt64(diff.TotalSeconds);
        }

        public ulong GetHoursAsTotalSeconds(uint hours)
        {
            return hours * 60 * 60;
        }
    }
}
