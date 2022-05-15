using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IResinTrackingInfoRepository
    {
        public Task AddOrUpdateResinCountAsync(ResinTrackingInfo resinTrackingInfo);
        public Task<ResinTrackingInfo?> GetResinTrackingInfoByDiscordIdAsync(ulong id);
    }
}
