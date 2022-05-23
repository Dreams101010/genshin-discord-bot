using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.HelperModels
{
    public struct ReminderRemoveModel
    {
        public ulong UserDiscordId { get; set; }
        public string? CategoryName { get; set; }
    }
}
