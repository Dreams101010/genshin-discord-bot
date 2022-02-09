using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;

namespace GenshinDiscordBotDomainLayer.DatabaseFacades
{
    public class ResinDatabaseFacade
    {
        private ICommand<AddOrUpdateResinInfoCommandParam, bool> 
            AddOrUpdateResinInfoCommand { get; }
        private IQuery<GetResinInfoByDiscordIdQueryParam, ResinTrackingInfo?> 
            GetResinInfoByDiscordIdQuery { get; }

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

        public async Task<ResinTrackingInfo?> GetResinForUser(ulong discordId)
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
            return nullableResinInfo.Value;
        }
    }
}
