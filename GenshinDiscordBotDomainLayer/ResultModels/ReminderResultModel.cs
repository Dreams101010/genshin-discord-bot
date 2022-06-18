using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ResultModels
{
    public struct ReminderResultModel
    {
        public DateTime SetupTime { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime ReminderTime { get; set; }
        public ulong Id { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }
    }
}
