using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.Localization;
using GenshinDiscordBotDomainLayer.ValidationLogic;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ReminderResponseGenerator
    {
        public Localization Localization { get; }
        public ReminderArgumentValidator ReminderArgumentValidator { get; }

        public ReminderResponseGenerator(Localization localization, 
            ReminderArgumentValidator reminderArgumentValidator)
        {
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
            ReminderArgumentValidator = reminderArgumentValidator ?? throw new ArgumentNullException(nameof(reminderArgumentValidator));
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

        internal string GetCheckInReminderSetupSuccessMessageWithCustomTime(UserLocale locale, TimeOnly timeOnly)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["CheckInReminderSetupSuccessMessageWithCustomTime"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["CheckInReminderSetupSuccessMessageWithCustomTime"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return string.Format(format, timeOnly);
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

        internal string GetReminderListString(UserLocale locale, List<ReminderResultModel> reminderList)
        {
            StringBuilder builder = new StringBuilder();
            if (reminderList.Count > 0)
            {
                string header = locale switch
                {
                    UserLocale.enGB => Localization.English["Reminder"]["ReminderListHeader"],
                    UserLocale.ruRU => Localization.Russian["Reminder"]["ReminderListHeader"],
                    _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
                };
                string entry = locale switch
                {
                    UserLocale.enGB => Localization.English["Reminder"]["ReminderListEntry"],
                    UserLocale.ruRU => Localization.Russian["Reminder"]["ReminderListEntry"],
                    _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
                };
                builder.AppendLine(header);
                foreach (var reminder in reminderList)
                {
                    builder.AppendLine(string.Format(entry, reminder.CategoryName,
                        reminder.SetupTime, reminder.Interval, reminder.ReminderTime));
                }
            }
            else
            {
                string emptyMessage = locale switch
                {
                    UserLocale.enGB => Localization.English["Reminder"]["ReminderListEmpty"],
                    UserLocale.ruRU => Localization.Russian["Reminder"]["ReminderListEmpty"],
                    _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
                };
                builder.AppendLine(emptyMessage);
            }
            return builder.ToString();
        }

        internal string GetSereniteaPotPlantHarvestSetupSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetSereniteaPotPlantHarvestCancelSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["SereniteaPotPlantHarvestCancelSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["SereniteaPotPlantHarvestCancelSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetSereniteaPotPlantHarvestCheckInReminderCancelNotFoundMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Reminder"]["GetSereniteaPotPlantHarvestCheckInReminderCancelNotFound"],
                UserLocale.ruRU => Localization.Russian["Reminder"]["GetSereniteaPotPlantHarvestCheckInReminderCancelNotFound"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        internal string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime(UserLocale locale, int days, int hours)
        {
            return locale switch
            {
                UserLocale.enGB => GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeEnglish(days, hours),
                UserLocale.ruRU => GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeRussian(days, hours),
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
        }

        private string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeEnglish(int days, int hours)
        {
            string format = Localization.English["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime"];
            return string.Format(format, days, hours);
        }

        private string GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTimeRussian(int days, int hours)
        {
            string format = Localization.Russian["Reminder"]["SereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime"];
            string daysString = GetReminderDaysAsStringRussian(days);
            string hoursString = GetReminderHoursAsStringRussian(hours);
            string unionString = Localization.Russian["LanguageSpecific"]["AndUnion"];
            if (daysString.Length == 0 || hoursString.Length == 0)
            {
                unionString = string.Empty;
            }
            return string.Format(format, daysString, unionString, hoursString);
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

        internal string GetUpdateOrCreateSereniteaPotPlantHarvestReminderValidationErrorMessage(UserLocale locale, int days, int hours)
        {
            // TODO: consider adding a non-throwing method to validation class and use it as
            // a catch-all
            if (!ReminderArgumentValidator
                    .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync_TimeValid(days, hours))
            {
                string format = locale switch
                {
                    UserLocale.enGB => Localization.English["Reminder"]["SereniteaPotPlantHarvestTimeInvalid"],
                    UserLocale.ruRU => Localization.Russian["Reminder"]["SereniteaPotPlantHarvestTimeInvalid"],
                    _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
                };
                return format;
            }
            return string.Empty;
        }
    }
}
