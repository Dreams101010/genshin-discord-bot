using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class UserResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        public UserResponseGenerator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public string GetUserSettingsList(User user)
        {
            return string.Format("Locale: {0}, Reminders state: {1}", user.Locale, user.RemindersOptIn);
        }

        public string GetListOfPossibleLocales()
        {
            return "Possible locales are: \n" +
                "ruRU \n" +
                "enGB \n";
        }

        public string GetLocaleErrorMessage()
        {
            return "Incorrect locale setting. Correct settings are ruRU and enGB";
        }

        public string GetLocaleSuccessMessage()
        {
            return "Locale has been set.";
        }

        public string GetEnableRemindersSuccessMessage()
        {
            return "Reminders have been enabled.";
        }

        public string GetDisableRemindersSuccessMessage()
        {
            return "Reminders have been disabled.";
        }
    }
}
