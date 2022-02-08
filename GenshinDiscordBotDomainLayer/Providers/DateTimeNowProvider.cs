using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotDomainLayer.Providers
{
    public class DateTimeNowProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
