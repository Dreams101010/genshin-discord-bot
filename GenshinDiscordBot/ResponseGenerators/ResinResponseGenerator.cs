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

        public string GetGetResinSuccessResponse(
            UserLocale locale, ResinInfoResultModel resultModel, string userName)
        {
            var resinCountFormat = Localization.GetLocalizedString("Resin",
                "ResinCount", locale);
            var timeToResin = Localization.GetLocalizedString("Resin",
                "ResinCount", locale);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format(resinCountFormat, userName, 
                resultModel.CurrentCount));
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

        public string GetGetResinErrorMessage(
            UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Resin",
                "GetResinErrorMessage", locale);
            return string.Format(format, userName);
        }

        public string GetSetResinValidationErrorMessage(
            UserLocale locale, int newResinValue, string userName)
        {
            // TODO: consider adding a non-throwing method to validation class and use it as
            // a catch-all
            if (!ResinCommandArgumentValidator.SetResinCount_ResinCountValid(newResinValue))
            {
                var format = Localization.GetLocalizedString("Resin",
                    "InvalidResinValue", locale);
                return string.Format(format, userName);
            }
            return string.Empty;
        }

        public string GetSetResinSuccessMessage(UserLocale locale, string userName)
        {
            var format = Localization.GetLocalizedString("Resin",
                "SetResinSuccessMessage", locale);
            return string.Format(format, userName);
        }
    }
}
