using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.ResultModels;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ResinResponseGenerator
    {
        public IDateTimeProvider DateTimeProvider { get; }
        public ResinResponseGenerator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
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

        public string GetSetResinErrorMessage()
        {
            return "Invalid resin value";
        }

        public string GetSetResinSuccessMessage()
        {
            return "Resin has been set.";
        }
    }
}
