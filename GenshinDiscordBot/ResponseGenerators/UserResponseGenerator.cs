﻿using System;
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
            string format = user.Locale switch
            {
                UserLocale.enGB => Localization.English["User"]["UserSettings"],
                UserLocale.ruRU => Localization.Russian["User"]["UserSettings"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format,
                userName,
                UserHelper.GetLanguageFromLocale(user.Locale), 
                UserHelper.GetReminderStateAsString(user.RemindersOptIn));
        }

        public string GetListOfPossibleLanguages(UserLocale locale, string userName)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["User"]["AvailableLanguages"],
                UserLocale.ruRU => Localization.Russian["User"]["AvailableLanguages"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, userName);
        }

        public string GetLanguageErrorMessage(UserLocale locale, string userName)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["User"]["LanguageErrorMessage"],
                UserLocale.ruRU => Localization.Russian["User"]["LanguageErrorMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, userName);
        }

        public string GetLanguageSuccessMessage(UserLocale locale, string userName)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["User"]["LanguageSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["User"]["LanguageSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, userName);
        }

        public string GetEnableRemindersSuccessMessage(UserLocale locale, string userName)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["User"]["EnableRemindersSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["User"]["EnableRemindersSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, userName);
        }

        public string GetDisableRemindersSuccessMessage(UserLocale locale, string userName)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["User"]["DisableRemindersSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["User"]["DisableRemindersSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, userName);
        }
    }
}
