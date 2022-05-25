using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public class ResinSqliteDatabaseInteractionHandler : SqliteDatabaseInteractionHandler, IResinDatabaseInteractionHandler
    {
        private IResinTrackingInfoRepository ResinRepository { get; }

        public ResinSqliteDatabaseInteractionHandler(
            IResinTrackingInfoRepository resinRepository, SqliteConnection connection)
            : base(connection)
        {
            ResinRepository = resinRepository ?? throw new ArgumentNullException(nameof(resinRepository));
        }

        public async Task<bool> SetResinForUserAsync(ulong discordId, int resinCount)
        {
            return await ExecuteInTransactionAsync(
                async () => await SetResinForUserFuncAsync(discordId, resinCount)
            );
        }

        // TODO : rewrite this, inject DateTime provider and business logic class
        private async Task<bool> SetResinForUserFuncAsync(ulong discordId, int resinCount)
        {
            var resinInfo = new ResinTrackingInfo()
            {
                StartCount = resinCount,
                StartTime = DateTime.Now.ToUniversalTime(),
                UserDiscordId = discordId
            };
            await ResinRepository.AddOrUpdateResinCountAsync(resinInfo);
            return true;
        }

        public async Task<ResinTrackingInfo> GetResinForUserAsync(ulong discordId)
        {
            return await ExecuteInTransactionAsync(
                async () => await GetResinForUserFuncAsync(discordId)
            );
        }

        private async Task<ResinTrackingInfo> GetResinForUserFuncAsync(ulong discordId)
        {
            return await ResinRepository.GetResinTrackingInfoByDiscordIdAsync(discordId);
        }
    }
}
