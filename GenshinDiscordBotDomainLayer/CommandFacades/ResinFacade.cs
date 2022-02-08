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
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;

namespace GenshinDiscordBotDomainLayer.CommandFacades
{
    public class ResinFacade
    {
        public UserDatabaseFacade UserDatabaseFacade { get; }
        public ResinDatabaseFacade ResinDatabaseFacade { get; }
        public ResinBusinessLogic ResinBusinessLogic { get; }
        public ResinCommandArgumentValidator ResinValidator { get; }

        public ResinFacade(
            UserDatabaseFacade userDatabaseFacade,
            ResinDatabaseFacade resinDatabaseFacade,
            ResinBusinessLogic resinBusinessLogic,
            ResinCommandArgumentValidator resinValidator)
        {
            UserDatabaseFacade = userDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(userDatabaseFacade));
            ResinDatabaseFacade = resinDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(resinDatabaseFacade));
            ResinBusinessLogic = resinBusinessLogic 
                ?? throw new ArgumentNullException(nameof(resinBusinessLogic));
            ResinValidator = resinValidator 
                ?? throw new ArgumentNullException(nameof(resinValidator));
        }

        public async Task<bool> SetResinForUser(ulong discordId, int resinCount)
        {
            ResinValidator.SetResinCount_Validate(resinCount);
            await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
            await ResinDatabaseFacade.SetResinForUser(discordId, resinCount);
            return true;
        }

        public async Task<ResinInfoResultModel?> GetResinForUser(ulong discordId)
        {
            var user = await UserDatabaseFacade.ReadUserAndCreateIfNotExistsAsync(discordId);
            var nullableResinInfo = await ResinDatabaseFacade.GetResinForUser(discordId);
            if (nullableResinInfo == null)
            {
                return null;
            }
            var resinInfo = nullableResinInfo.Value;
            var result = ResinBusinessLogic.GetResinResult(user, resinInfo);
            return result;
        }
    }
}
