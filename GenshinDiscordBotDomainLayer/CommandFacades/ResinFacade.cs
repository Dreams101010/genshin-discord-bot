using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DatabaseFacades;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;
using GenshinDiscordBotDomainLayer.ResultModels;

namespace GenshinDiscordBotDomainLayer.CommandFacades
{
    public class ResinFacade
    {
        public UserDatabaseFacade UserDatabaseFacade { get; }
        public ResinDatabaseFacade ResinDatabaseFacade { get; }
        public ResinFacade(
            UserDatabaseFacade userDatabaseFacade,
            ResinDatabaseFacade resinDatabaseFacade)
        {
            UserDatabaseFacade = userDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(userDatabaseFacade));
            ResinDatabaseFacade = resinDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(resinDatabaseFacade));
        }

        public async Task<bool> SetResinForUser(ulong discordId, int resinCount)
        {
            await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
            await ResinDatabaseFacade.SetResinForUser(discordId, resinCount);
            return true;
        }

        public async Task<ResinInfoResultModel?> GetResinForUser(ulong discordId)
        {
            await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
            return await ResinDatabaseFacade.GetResinForUser(discordId);
        }
    }
}
