using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Helpers;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DataModels
{
    public struct ReminderDataModel
    {
        public ulong UserDiscordId { get; set; }
        public string UserLocale { get; set; }
        public bool RemindersOptInFlag { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong Interval { get; set; }
        public ulong ReminderTime { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }
        public bool RecurrentFlag { get; set; }

        public Reminder ToReminderDomain()
        {
            return new Reminder
            {
                CategoryName = this.CategoryName,
                Message = this.Message,
                ReminderTime = this.ReminderTime,
                ChannelId = this.ChannelId,
                Interval = this.Interval,
                GuildId = this.GuildId,
                RemindersOptInFlag = this.RemindersOptInFlag,
                RecurrentFlag = this.RecurrentFlag,
                UserDiscordId = this.UserDiscordId,
                UserLocale = EnumConversionHelper.UserLocaleFromString(this.UserLocale)
            };
        }
    }
}
