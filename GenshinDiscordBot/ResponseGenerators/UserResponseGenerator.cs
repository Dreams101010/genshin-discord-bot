using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class UserResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        private UserHelper UserHelper { get; }
        public Localization Localization { get; }

        public UserResponseGenerator(IDateTimeProvider dateTimeProvider, UserHelper userHelper,
            Localization localization)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public string GetUserSettingsList(User user, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "UserSettings", user.Locale);
            return string.Format(format,
                userName,
                UserHelper.GetLanguageFromLocale(user.Locale), 
                UserHelper.GetReminderStateAsString(user.RemindersOptIn));
        }

        public string GetListOfPossibleLanguages(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "AvailableLanguages", locale);
            return string.Format(format, userName);
        }

        public string GetLanguageErrorMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "LanguageErrorMessage", locale);
            return string.Format(format, userName);
        }

        public string GetLanguageSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "LanguageSuccessMessage", locale);
            return string.Format(format, userName);
        }

        public string GetEnableRemindersSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "EnableRemindersSuccessMessage", locale);
            return string.Format(format, userName);
        }

        public string GetDisableRemindersSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("User",
                "DisableRemindersSuccessMessage", locale);
            return string.Format(format, userName);
        }
    }
}
