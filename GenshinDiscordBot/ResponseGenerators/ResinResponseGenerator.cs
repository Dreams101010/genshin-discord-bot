using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ResinResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        private ResinCommandArgumentValidator ResinCommandArgumentValidator { get; }
        public Localization Localization { get; }

        public ResinResponseGenerator(
            IDateTimeProvider dateTimeProvider,
            ResinCommandArgumentValidator resinCommandArgumentValidator,
            Localization localization)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            ResinCommandArgumentValidator = resinCommandArgumentValidator 
                ?? throw new ArgumentNullException(nameof(resinCommandArgumentValidator));
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public string GetGetResinSuccessResponse(UserLocale locale, ResinInfoResultModel resultModel)
        {
            string resinCountFormat = locale switch
            {
                UserLocale.enGB => Localization.English["Resin"]["ResinCount"],
                UserLocale.ruRU => Localization.Russian["Resin"]["ResinCount"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            string timeToResin = locale switch
            {
                UserLocale.enGB => Localization.English["Resin"]["TimeToResin"],
                UserLocale.ruRU => Localization.Russian["Resin"]["TimeToResin"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format(resinCountFormat, resultModel.CurrentCount));
            var keys = resultModel.CompletionTimes.Keys.OrderBy(x => x);
            foreach (var key in keys)
            {
                builder.AppendLine(string.Format(timeToResin,
                    key,
                    resultModel.CompletionTimes[key].TimeSpanToResin,
                    resultModel.CompletionTimes[key].TimeToResinUtc));
            }
            return builder.ToString();
        }

        public string GetGetResinErrorMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Resin"]["GetResinErrorMessage"],
                UserLocale.ruRU => Localization.Russian["Resin"]["GetResinErrorMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }

        public string GetSetResinValidationErrorMessage(UserLocale locale, int newResinValue)
        {
            // TODO: consider adding a non-throwing method to validation class and use it as
            // a catch-all
            if (!ResinCommandArgumentValidator.SetResinCount_ResinCountValid(newResinValue))
            {
                string format = locale switch
                {
                    UserLocale.enGB => Localization.English["Resin"]["InvalidResinValue"],
                    UserLocale.ruRU => Localization.Russian["Resin"]["InvalidResinValue"],
                    _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
                };
                return format;
            }
            return string.Empty;
        }

        public string GetSetResinSuccessMessage(UserLocale locale)
        {
            string format = locale switch
            {
                UserLocale.enGB => Localization.English["Resin"]["SetResinSuccessMessage"],
                UserLocale.ruRU => Localization.Russian["Resin"]["SetResinSuccessMessage"],
                _ => throw new NotImplementedException("Invalid state of UserLocale enum"),
            };
            return format;
        }
    }
}
