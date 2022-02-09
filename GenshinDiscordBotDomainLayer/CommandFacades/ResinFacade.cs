using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DatabaseFacades;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.ErrorHandlers;

namespace GenshinDiscordBotDomainLayer.CommandFacades
{
    public class ResinFacade
    {
        private UserDatabaseFacade UserDatabaseFacade { get; }
        private ResinDatabaseFacade ResinDatabaseFacade { get; }
        private ResinBusinessLogic ResinBusinessLogic { get; }
        private ResinCommandArgumentValidator ResinValidator { get; }
        private FacadeErrorHandler ErrorHandler { get; }

        public ResinFacade(
            UserDatabaseFacade userDatabaseFacade,
            ResinDatabaseFacade resinDatabaseFacade,
            ResinBusinessLogic resinBusinessLogic,
            ResinCommandArgumentValidator resinValidator,
            FacadeErrorHandler errorHandler)
        {
            UserDatabaseFacade = userDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(userDatabaseFacade));
            ResinDatabaseFacade = resinDatabaseFacade 
                ?? throw new ArgumentNullException(nameof(resinDatabaseFacade));
            ResinBusinessLogic = resinBusinessLogic 
                ?? throw new ArgumentNullException(nameof(resinBusinessLogic));
            ResinValidator = resinValidator 
                ?? throw new ArgumentNullException(nameof(resinValidator));
            ErrorHandler = errorHandler 
                ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public async Task<bool> SetResinForUser(ulong discordId, int resinCount)
        {
            try
            {
                ResinValidator.SetResinCount_Validate(resinCount);
                await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
                await ResinDatabaseFacade.SetResinForUser(discordId, resinCount);
                return true;
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public async Task<ResinInfoResultModel?> GetResinForUser(ulong discordId)
        {
            try
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
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }
    }
}
