using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.ValidationLogic;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ResinResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        private ResinCommandArgumentValidator ResinCommandArgumentValidator { get; }

        public ResinResponseGenerator(
            IDateTimeProvider dateTimeProvider,
            ResinCommandArgumentValidator resinCommandArgumentValidator)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            ResinCommandArgumentValidator = resinCommandArgumentValidator 
                ?? throw new ArgumentNullException(nameof(resinCommandArgumentValidator));
        }

        public string GetGetResinSuccessResponse(ResinInfoResultModel resultModel)
        {
            string format = "Your resin count is {0}. " +
                        "Time to full resin is {1} " +
                        "({2} UTC)";
            return string.Format(format, resultModel.CurrentCount, 
                        resultModel.TimeToFullResin, 
                        resultModel.CompletionTime);
        }

        public string GetGetResinErrorMessage()
        {
            return "Could not get resin count for you. Perhaps resin has not been set?";
        }

        public string GetSetResinValidationErrorMessage(int newResinValue)
        {
            // TODO: consider adding a non-throwing method to validation class and use it as
            // a catch-all
            if (!ResinCommandArgumentValidator.SetResinCount_ResinCountValid(newResinValue))
            {
                return "Invalid resin value.";
            }
            return null;
        }

        public string GetSetResinSuccessMessage()
        {
            return "Resin has been set.";
        }
    }
}
