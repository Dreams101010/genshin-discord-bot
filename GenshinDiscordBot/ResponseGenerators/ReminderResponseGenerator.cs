using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ReminderResponseGenerator
    {
        public Localization Localization { get; }

        public ReminderResponseGenerator(Localization localization)
        {
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }
        public string GetArtifactReminderSetupSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["ArtifactReminderSetupSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["ArtifactReminderSetupSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        public string GetArtifactReminderCancelSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["ArtifactReminderCancelSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["ArtifactReminderCancelSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        public string GetArtifactReminderCancelNotFoundMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["ArtifactReminderCancelNotFoundMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["ArtifactReminderCancelNotFoundMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetCheckInReminderSetupSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["CheckInReminderSetupSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["CheckInReminderSetupSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetCheckInReminderCancelSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["CheckInReminderCancelSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["CheckInReminderCancelSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetCheckInReminderCancelNotFoundMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["CheckInReminderCancelNotFoundMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["CheckInReminderCancelNotFoundMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }
    }
}
