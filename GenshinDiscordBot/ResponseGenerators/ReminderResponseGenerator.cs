﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.Localization;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using System.Globalization;
using GenshinDiscordBotDomainLayer.Contexts;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ReminderResponseGenerator
    {
        public Localization Localization { get; }
        public ReminderArgumentValidator ReminderArgumentValidator { get; }
        public RequestContext Context { get; }

        public ReminderResponseGenerator(Localization localization, 
            ReminderArgumentValidator reminderArgumentValidator,
            RequestContext requestContext)
        {
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
            ReminderArgumentValidator = reminderArgumentValidator ?? throw new ArgumentNullException(nameof(reminderArgumentValidator));
            Context = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public string GetArtifactReminderSetupSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder", 
                "ArtifactReminderSetupSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetArtifactReminderSetupSuccessMessageWithCustomTime(
            UserLocale locale, TimeOnly timeOnly, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                    "ArtifactReminderSetupSuccessMessageWithCustomTime", locale);
            return string.Format(format, userName, timeOnly);
        }

        public string GetArtifactReminderCancelSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ArtifactReminderCancelSuccessMessage", locale);
            return string.Format(format, userName);
        }

        public string GetArtifactReminderCancelNotFoundMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ArtifactReminderCancelNotFoundMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetCheckInReminderSetupSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "CheckInReminderSetupSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetCheckInReminderSetupSuccessMessageWithCustomTime(
            UserLocale locale, TimeOnly timeOnly, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "CheckInReminderSetupSuccessMessageWithCustomTime", locale);
            return string.Format(format, userName, timeOnly);
        }

        internal string GetReminderTimeInvalid(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderTimeInvalid", locale);
            return string.Format(format, userName);
        }

        internal string GetReminderDateTimeInvalid(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderDateTimeInvalid", locale);
            return string.Format(format, userName);
        }

        internal string GetReminderTimeSpanInvalid(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderTimeSpanInvalid", locale);
            return string.Format(format, userName);
        }

        internal string GetReminderDateTimeNotInFuture(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderDateTimeNotInFuture", locale);
            return string.Format(format, userName);
        }

        internal string GetCheckInReminderCancelSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "CheckInReminderCancelSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetCheckInReminderCancelNotFoundMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "CheckInReminderCancelNotFoundMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetReminderListString(
            UserLocale locale, List<ReminderResultModel> reminderList, string userName, DateTime currentUtc)
        {
            string localizedYes = Localization.GetLocalizedString("General", "Yes", locale);
            string localizedNo = Localization.GetLocalizedString("General", "No", locale);
            CultureInfo culture = Context.GetUserCulture();
            StringBuilder builder = new StringBuilder();
            if (reminderList.Count > 0)
            {
                var header = Localization.GetLocalizedString("Reminder",
                    "ReminderListHeader", locale);
                header = string.Format(header, userName);
                var entry = Localization.GetLocalizedString("Reminder",
                    "ReminderListEntry", locale);
                builder.AppendLine(header);
                foreach (var reminder in reminderList)
                {
                    string reminderRecurrenceString = reminder.IsRecurrent ? localizedYes : localizedNo;
                    TimeSpan timeRemaining = reminder.GetTimeRemaining(currentUtc);
                    builder.AppendLine(string.Format(entry, reminder.Id, reminder.CategoryName, reminder.Message,
                        reminder.SetupTime.ToString(culture), reminder.Interval, 
                        reminder.ReminderTime.ToString(culture), timeRemaining.ToString(@"c", culture), reminderRecurrenceString));
                }
            }
            else
            {
                var emptyMessage = Localization.GetLocalizedString("Reminder",
                    "ReminderListEmpty", locale);
                emptyMessage = string.Format(emptyMessage, userName);
                builder.AppendLine(emptyMessage);
            }
            return builder.ToString();
        }

        internal string GetSereniteaPotPlantHarvestSetupSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "SereniteaPotPlantHarvestSetupSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetSereniteaPotPlantHarvestCancelSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "SereniteaPotPlantHarvestCancelSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetSereniteaPotPlantHarvestCheckInReminderCancelNotFoundMessage(
            UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "GetSereniteaPotPlantHarvestCheckInReminderCancelNotFound", locale);
            return string.Format(format, userName);
        }

        internal string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime(
            UserLocale locale, int days, int hours, string userName)
        {
            return locale switch
            {
                UserLocale.enGB => GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeEnglish(
                    days, hours, userName),
                UserLocale.ruRU => GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeRussian(
                    days, hours, userName),
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
        }

        private string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeEnglish(
            int days, int hours, string userName)
        {
            string format = Localization.English["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime"];
            return string.Format(format, userName, days, hours);
        }

        private string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeRussian(
            int days, int hours, string userName)
        {
            string format = Localization.Russian["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime"];
            string daysString = GetReminderDaysAsStringRussian(days);
            string hoursString = GetReminderHoursAsStringRussian(hours);
            string unionString = Localization.Russian["LanguageSpecific"]["AndUnion"];
            if (daysString.Length == 0 || hoursString.Length == 0)
            {
                unionString = string.Empty;
            }
            return string.Format(format, userName, daysString, unionString, hoursString);
        }

        private string GetReminderHoursAsStringRussian(int hours)
        {
            if (hours == 0)
            {
                return string.Empty;
            }
            var format = hours switch
            {
                0 => string.Empty,
                1 or 21 => Localization.Russian["LanguageSpecific"]["HoursSingular"],
                >= 2 and <= 4 or 22 or 23 => Localization.Russian["LanguageSpecific"]["HoursPlural1"],
                < 0 or > 23 => throw new ArgumentException("Invalid hours value"),
                _ => Localization.Russian["LanguageSpecific"]["HoursPlural2"],
            };
            return hours + format;
        }

        private string GetReminderDaysAsStringRussian(int days)
        {
            if (days == 0)
            {
                return string.Empty;
            }
            var format = days switch
            {
                0 => string.Empty,
                11 => Localization.Russian["LanguageSpecific"]["DaysPlural2"],
                >= 12 and <= 14 => Localization.Russian["LanguageSpecific"]["DaysPlural2"],
                _ when days.ToString().EndsWith('1')
                    => Localization.Russian["LanguageSpecific"]["DaysSingular"],
                _ when (days.ToString().EndsWith('2') || days.ToString().EndsWith('3') || days.ToString().EndsWith('4'))
                    => Localization.Russian["LanguageSpecific"]["DaysPlural1"],
                < 0 => throw new ArgumentException("The number of days cannot be negative"),
                _ => Localization.Russian["LanguageSpecific"]["DaysPlural2"],

            };
            return days + format;
        }

        internal string GetUpdateOrCreateSereniteaPotPlantHarvestReminderValidationErrorMessage(UserLocale locale, int days, int hours, string userName)
        {
            if (!ReminderArgumentValidator
                    .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync_TimeValid(days, hours))
            {
                var format = Localization.GetLocalizedString("Reminder",
                    "SereniteaPotPlantHarvestTimeInvalid", locale);
                return string.Format(format, userName);
            }
            return string.Empty;
        }

        internal string GetReminderRemoveByIdSuccessMessage(UserLocale locale)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderRemoveByIdSuccessMessage", locale);
            return format;
        }
        internal string GetReminderRemoveByIdNotFoundMessage(UserLocale locale)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ReminderRemoveByIdNotFoundMessage", locale);
            return format;
        }

        internal string GetParametricTransformerReminderSetupSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ParametricTransformerReminderSetupSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetParametricTransformerReminderSetupSuccessMessageWithCustomTime(
            UserLocale locale, int days, int hours, string userName)
        {
            return locale switch
            {
                UserLocale.enGB => GetParametricTransformerReminderSetupSuccessMessageWithCustomTimeEnglish(
                    days, hours, userName),
                UserLocale.ruRU => GetParametricTransformerReminderSetupSuccessMessageWithCustomTimeRussian(
                    days, hours, userName),
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
        }

        private string GetParametricTransformerReminderSetupSuccessMessageWithCustomTimeEnglish(
            int days, int hours, string userName)
        {
            var format = Localization.English["Reminder"]["ParametricTransformerReminderSetupSuccessMessageWithCustomTime"];
            return string.Format(format, userName, days, hours);
        }
        internal string GetParametricTransformerReminderSetupSuccessMessageWithCustomTimeRussian(
            int days, int hours, string userName)
        {
            string format = Localization.Russian["Reminder"]["ParametricTransformerReminderSetupSuccessMessageWithCustomTime"];
            string daysString = GetReminderDaysAsStringRussian(days);
            string hoursString = GetReminderHoursAsStringRussian(hours);
            string unionString = Localization.Russian["LanguageSpecific"]["AndUnion"];
            if (daysString.Length == 0 || hoursString.Length == 0)
            {
                unionString = string.Empty;
            }
            return string.Format(format, userName, daysString, unionString, hoursString);
        }

        internal string GetParametricTransformerReminderCancelSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ParametricTransformerReminderCancelSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetParametricTransformerReminderCancelNotFoundMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "ParametricTransformerReminderCancelNotFoundMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetReminderSetupSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Reminder",
                "GenericReminderAddSuccessMessage", locale);
            return string.Format(format, userName);
        }

        internal string GetUpdateOrCreateParametricTransformerReminderCustomTimeValidationErrorMessage
            (UserLocale locale, int days, int hours, string userName)
        {
            if (!ReminderArgumentValidator
                    .UpdateOrCreateParametricTransformerReminderAsync_TimeValid(days, hours))
            {
                var format = Localization.GetLocalizedString("Reminder",
                    "ParametricTransformerTimeInvalid", locale);
                return string.Format(format, userName);
            }
            return string.Empty;
        }
    }
}
