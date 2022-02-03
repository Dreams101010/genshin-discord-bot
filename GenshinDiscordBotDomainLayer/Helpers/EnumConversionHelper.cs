using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Helpers
{
    public static class EnumConversionHelper
    {
        public static string UserLocaleToString(UserLocale locale)
        {
            return locale switch
            {
                UserLocale.ruRU => "ru-RU",
                UserLocale.enGB => "en-GB",
                _ => throw new ArgumentException(nameof(UserLocale))
            };
        }

        public static UserLocale UserLocaleFromString(string str)
        {
            return str switch
            {
                "ru-RU" => UserLocale.ruRU,
                "en-GB" => UserLocale.enGB,
                _ => throw new ArgumentException(nameof(UserLocale))
            };
        }
    }
}
