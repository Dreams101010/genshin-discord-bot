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
		public bool IsLocale(string str)
		{
			str = str.ToLower();
			if (str == "ruru" || str == "engb")
			{
				return true;
			}
			return false;
		}

		public UserLocale GetLocaleFromString(string str)
		{
			return str.ToLower() switch
			{
				"ruru" => UserLocale.ruRU,
				"engb" => UserLocale.enGB,
				_ => throw new ArgumentException("Invalid locale value")
			};
		}
	}
}
