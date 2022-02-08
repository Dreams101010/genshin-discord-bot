using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.DataProviders;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class ResinBusinessLogic
    {
        public ResinDataProvider DataProvider { get; }
        private IDateTimeProvider DateTimeProvider { get; }

        public ResinBusinessLogic(ResinDataProvider dataProvider, IDateTimeProvider dateTimeProvider)
        {
            DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public ResinInfoResultModel? GetResinResult(User user, ResinTrackingInfo resinInfo)
        {
            var utcNow = DateTimeProvider.GetDateTime().ToUniversalTime();
            int currentCount = GetResinCount(resinInfo, utcNow);
            TimeSpan timeToCompletion = GetTimeToFullResin(currentCount);
            DateTime completionTime = GetCompletionTime(user, utcNow, timeToCompletion);
            var result = new ResinInfoResultModel
            {
                CurrentCount = currentCount,
                TimeToFullResin = timeToCompletion,
                CompletionTime = completionTime,
            };
            return result;
        }

        private int GetDifferenceInMinutes(DateTime first, DateTime second)
        {
            return Convert.ToInt32(Math.Floor((first - second).TotalMinutes));
        }

        private int GetResinCount(ResinTrackingInfo resinInfo, DateTime currentTimeInUtc)
        {
            var utcStartTime = resinInfo.StartTime;
            var differenceInMinutesFromStartToNow = 
                GetDifferenceInMinutes(currentTimeInUtc, utcStartTime);
            return Math.Min(DataProvider.MaxResin,
                resinInfo.StartCount + 
                differenceInMinutesFromStartToNow / DataProvider.MinutesPerOneResin);
        }

        private TimeSpan GetTimeToFullResin(int currentCount)
        {
            if (currentCount >= DataProvider.MaxResin) // is complete
            {
                return new TimeSpan(0, 0, 0);
            }
            else
            {
                int resinCountDiff = DataProvider.MaxResin - currentCount;
                // must be a better way to initialize this
                return new TimeSpan(0, DataProvider.MinutesPerOneResin, 0)
                    .Multiply(resinCountDiff);
            }
        }

        private DateTime GetCompletionTime(User user, DateTime baseDateTime, TimeSpan timeToCompletion)
        {
            // TODO: convert utcNow to user timezone
            return baseDateTime + timeToCompletion;
        }
    }
}
