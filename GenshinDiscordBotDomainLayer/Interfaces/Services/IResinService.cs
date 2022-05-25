using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.ResultModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services
{
    public interface IResinService
    {
        public Task<bool> SetResinForUserAsync(ulong discordId, int resinCount);
        public Task<ResinInfoResultModel> GetResinForUserAsync(ulong discordId);
    }
}
