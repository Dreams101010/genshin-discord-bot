using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotUI.Helpers;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class UserResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        private UserHelper UserHelper { get; }

        public UserResponseGenerator(IDateTimeProvider dateTimeProvider, UserHelper userHelper)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
        }

        public string GetUserSettingsList(User user)
        {
            return string.Format(@"Language: {0} 
Reminders: {1}", 
                UserHelper.GetLanguageFromLocale(user.Locale), 
                UserHelper.GetReminderStateAsString(user.RemindersOptIn));
        }

        public string GetListOfPossibleLanguages()
        {
            return "Available languages are: \n" +
                "ru \n" +
                "en \n";
        }

        public string GetLanguageErrorMessage()
        {
            return "Incorrect language setting. Correct settings are ru and en";
        }

        public string GetLanguageSuccessMessage()
        {
            return "Language has been set.";
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
