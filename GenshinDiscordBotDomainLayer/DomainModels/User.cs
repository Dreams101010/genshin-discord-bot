using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels
{
    public enum UserLocale
    {
        enGB, ruRU
    }
    public struct User
    {
        public ulong DiscordId { get; set; }
        public string Location { get; set; }
        public UserLocale Locale { get; set; }

        public static User GetDefaultUser()
        {
            return new User
            {
                DiscordId = 0,
                Location = "Not specified",
                Locale = UserLocale.enGB,
            };
        }
    }
}
