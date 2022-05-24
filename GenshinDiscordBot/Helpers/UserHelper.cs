using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotUI.Helpers
{
    public class UserHelper
    {
		public bool IsLocaleOrLanguage(string str)
		{
			str = str.ToLower();
			if (str == "ruru" || str == "engb" || str == "ru" || str == "en")
			{
				return true;
			}
			return false;
		}

		public UserLocale GetLocaleFromString(string str)
		{
			return str.ToLower() switch
			{
				"ruru" or "ru" => UserLocale.ruRU,
				"engb" or "en" => UserLocale.enGB,
				_ => throw new ArgumentException("Invalid locale value")
			};
		}

        public string GetLanguageFromLocale(UserLocale locale)
		{
			return locale switch
			{
				UserLocale.enGB => "en",
				UserLocale.ruRU => "ru",
				_ => throw new NotImplementedException("Invalid state of UserLocale enum"),
			};
		}

		public string GetReminderStateAsString(bool reminderState)
        {
			if (reminderState)
            {
				return "on";
            }
			else
            {
				return "off";
            }
        }
	}
}
