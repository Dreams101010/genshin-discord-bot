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

namespace GenshinDiscordBotDomainLayer.DatabaseFacades
{
    public class ResinDatabaseFacade
    {
        const int MAX_RESIN = 160;
        const int TIME_PER_ONE_RESIN_IN_MINUTES = 8;
        ICommand<AddOrUpdateResinInfoCommandParam, bool> AddOrUpdateResinInfoCommand { get; }
        IQuery<GetResinInfoByDiscordIdQueryParam, ResinTrackingInfo?> GetResinInfoByDiscordIdQuery { get; }

        public ResinDatabaseFacade(
            ICommand<AddOrUpdateResinInfoCommandParam, bool> addOrUpdateResinInfoCommand,
            IQuery<GetResinInfoByDiscordIdQueryParam, ResinTrackingInfo?> getResinInfoByDiscordIdQuery)
        {
            AddOrUpdateResinInfoCommand = addOrUpdateResinInfoCommand 
                ?? throw new ArgumentNullException(nameof(addOrUpdateResinInfoCommand));
            GetResinInfoByDiscordIdQuery = getResinInfoByDiscordIdQuery 
                ?? throw new ArgumentNullException(nameof(getResinInfoByDiscordIdQuery));
        }

        // TODO : rewrite this, inject DateTime provider and business logic class
        public async Task<bool> SetResinForUser(ulong discordId, int resinCount)
        {
            var param = new AddOrUpdateResinInfoCommandParam()
            {
                ResinInfo = new ResinTrackingInfo()
                {
                    StartCount = resinCount,
                    StartTime = DateTime.Now.ToUniversalTime(),
                    UserDiscordId = discordId
                }
            };
            await AddOrUpdateResinInfoCommand.ExecuteAsync(param);
            return true;
        }

        public async Task<ResinInfoResultModel?> GetResinForUser(ulong discordId)
        {
            var param = new GetResinInfoByDiscordIdQueryParam()
            {
                DiscordId = discordId
            };
            var nullableResinInfo = await GetResinInfoByDiscordIdQuery.QueryAsync(param, retry: true);
            if (!nullableResinInfo.HasValue)
            {
                return null;
            }
            var resinInfo = nullableResinInfo.Value;
            var utcNow = DateTime.Now.ToUniversalTime();
            var utcStartTime = resinInfo.StartTime;
            var diffInMinutes = Convert.ToInt32(Math.Floor((utcNow - utcStartTime).TotalMinutes));
            var currentCount = Math.Min(MAX_RESIN, 
                resinInfo.StartCount + diffInMinutes / TIME_PER_ONE_RESIN_IN_MINUTES);
            TimeSpan timeToCompletion;
            if (currentCount >= MAX_RESIN) // complete
            {
                timeToCompletion = new TimeSpan(0, 0, 0); 
            }
            else
            {
                int resinCountDiff = MAX_RESIN - currentCount;
                // must be a better way to initialize this
                timeToCompletion = new TimeSpan(0, TIME_PER_ONE_RESIN_IN_MINUTES, 0).Multiply(resinCountDiff);
            }
            var result = new ResinInfoResultModel
            {
                CurrentCount = currentCount,
                TimeToFullResin = timeToCompletion,
                CompletionTime = DateTime.Now.ToUniversalTime() + timeToCompletion,
            };
            return result;
        }
    }
}
