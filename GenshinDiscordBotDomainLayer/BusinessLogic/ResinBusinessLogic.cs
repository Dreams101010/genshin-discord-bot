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

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class ResinBusinessLogic
    {
        const int MAX_RESIN = 160;
        const int TIME_PER_ONE_RESIN_IN_MINUTES = 8;

        private IDateTimeProvider DateTimeProvider { get; }

        public ResinBusinessLogic(IDateTimeProvider dateTimeProvider)
        {
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
            return Math.Min(MAX_RESIN,
                resinInfo.StartCount + 
                differenceInMinutesFromStartToNow / TIME_PER_ONE_RESIN_IN_MINUTES);
        }

        private TimeSpan GetTimeToFullResin(int currentCount)
        {
            if (currentCount >= MAX_RESIN) // is complete
            {
                return new TimeSpan(0, 0, 0);
            }
            else
            {
                int resinCountDiff = MAX_RESIN - currentCount;
                // must be a better way to initialize this
                return new TimeSpan(0, TIME_PER_ONE_RESIN_IN_MINUTES, 0).Multiply(resinCountDiff);
            }
        }

        private DateTime GetCompletionTime(User user, DateTime baseDateTime, TimeSpan timeToCompletion)
        {
            // TODO: convert utcNow to user timezone
            return baseDateTime + timeToCompletion;
        }
    }
}
