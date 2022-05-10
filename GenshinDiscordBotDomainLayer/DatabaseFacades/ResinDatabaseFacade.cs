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
        public IResinTrackingInfoRepository ResinTrackingInfoRepository { get; }

        public ResinDatabaseFacade(IResinTrackingInfoRepository resinTrackingInfoRepository)
        {
            ResinTrackingInfoRepository = resinTrackingInfoRepository ?? throw new ArgumentNullException(nameof(resinTrackingInfoRepository));
        }

        // TODO : rewrite this, inject DateTime provider and business logic class
        public bool SetResinForUser(ulong discordId, int resinCount)
        {
            var resinInfo = new ResinTrackingInfo()
            {
                StartCount = resinCount,
                StartTime = DateTime.Now.ToUniversalTime(),
                UserDiscordId = discordId
            };
            ResinTrackingInfoRepository.AddOrUpdateResinCount(resinInfo);
            return true;
        }

        public ResinTrackingInfo? GetResinForUser(ulong discordId)
        {
            var nullableResinInfo = ResinTrackingInfoRepository.GetResinTrackingInfoByDiscordId(discordId);
            if (!nullableResinInfo.HasValue)
            {
                return null;
            }
            return nullableResinInfo.Value;
        }
    }
}
