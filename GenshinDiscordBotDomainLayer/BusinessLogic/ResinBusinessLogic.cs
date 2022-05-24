using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.DataProviders;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class ResinBusinessLogic
    {
        private ResinDataProvider DataProvider { get; }
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
            Dictionary<int, TimeToResin> completionTimes = GetCompletionTimes(currentCount, utcNow);
            var result = new ResinInfoResultModel
            {
                CurrentCount = currentCount,
                CompletionTimes = completionTimes,
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

        private TimeSpan GetTimeSpanToResinCount(int currentCount, int desiredCount)
        {
            if (desiredCount < 0 || desiredCount > DataProvider.MaxResin)
            {
                throw new ArgumentOutOfRangeException(nameof(desiredCount));
            }
            if (currentCount >= DataProvider.MaxResin 
                || desiredCount <= currentCount) // is complete
            {
                return TimeSpan.Zero;
            }
            else
            {
                int resinCountDiff = desiredCount - currentCount;
                // must be a better way to initialize this
                return TimeSpan.FromMinutes(DataProvider.MinutesPerOneResin)
                    .Multiply(resinCountDiff);
            }
        }

        private Dictionary<int, TimeToResin> GetCompletionTimes(int currentCount, 
            DateTime currentTimeUtc)
        {
            DateTime currentLocalTime = currentTimeUtc.ToLocalTime();
            var result = new Dictionary<int, TimeToResin>();
            for (int i = 0; i <= DataProvider.MaxResin; i += 20)
            {
                if (i <= currentCount)
                {
                    continue;
                }
                var timeSpanToIResin = GetTimeSpanToResinCount(currentCount, i);
                var timeToResin = new TimeToResin
                {
                    TimeSpanToResin = timeSpanToIResin,
                    TimeToResinUtc = currentLocalTime + timeSpanToIResin,
                };
                result.Add(i, timeToResin);
            }
            return result;
        }
    }
}
