using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.Repositories;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Parameters.Query;
using GenshinDiscordBotDomainLayer.DomainModels;
using Serilog;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Queries
{
    public class GetResinInfoByDiscordIdQuery
        : SQLiteAbstractQuery<GetResinInfoByDiscordIdQueryParam, ResinTrackingInfo?>
    {
        private ResinTrackingInfoRepository ResinRepository { get; }
        public GetResinInfoByDiscordIdQuery(
            SQLiteConnectionProvider connectionProvider,
            ILogger logger,
            ResinTrackingInfoRepository resinRepository)
            : base(connectionProvider, logger)
        {
            ResinRepository = resinRepository
                ?? throw new ArgumentNullException(nameof(resinRepository));
        }

        protected override async Task<ResinTrackingInfo?> PayloadAsync(GetResinInfoByDiscordIdQueryParam param)
        {
            var resinTrackingInfoDataModel = await ResinRepository.GetResinTrackingInfoByDiscordIdAsync(param.DiscordId);
            if (resinTrackingInfoDataModel.HasValue)
            {
                var resinTrackingInfo = resinTrackingInfoDataModel.Value;
                return resinTrackingInfo.ToResinTrackingInfo();
            }
            else
            {
                return null;
            }
        }
    }
}
