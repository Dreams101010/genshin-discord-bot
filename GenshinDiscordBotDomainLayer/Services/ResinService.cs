using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.ErrorHandlers;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class ResinService
    {
        private IUserDatabaseInteractionHandler UserDatabaseInteractionHandler { get; }
        private IResinDatabaseInteractionHandler ResinDatabaseInteractionHandler { get; }
        private ResinBusinessLogic ResinBusinessLogic { get; }
        private ResinCommandArgumentValidator ResinValidator { get; }
        private FacadeErrorHandler ErrorHandler { get; }

        public ResinService(IUserDatabaseInteractionHandler userDatabaseInteractionHandler,
            IResinDatabaseInteractionHandler resinDatabaseInteractionHandler,
            ResinBusinessLogic resinBusinessLogic,
            ResinCommandArgumentValidator resinValidator,
            FacadeErrorHandler errorHandler)
        {
            UserDatabaseInteractionHandler = userDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(userDatabaseInteractionHandler));
            ResinDatabaseInteractionHandler = resinDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(resinDatabaseInteractionHandler));
            ResinBusinessLogic = resinBusinessLogic 
                ?? throw new ArgumentNullException(nameof(resinBusinessLogic));
            ResinValidator = resinValidator 
                ?? throw new ArgumentNullException(nameof(resinValidator));
            ErrorHandler = errorHandler 
                ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public async virtual Task<bool> SetResinForUserAsync(ulong discordId, int resinCount)
        {
            try
            {
                ResinValidator.SetResinCount_Validate(resinCount);
                await UserDatabaseInteractionHandler.CreateUserIfNotExistsAsync(discordId);
                await ResinDatabaseInteractionHandler.SetResinForUserAsync(discordId, resinCount);
                return true;
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public async virtual Task<ResinInfoResultModel?> GetResinForUserAsync(ulong discordId)
        {
            try
            {
                var user = await UserDatabaseInteractionHandler.ReadUserAndCreateIfNotExistsAsync(discordId);
                var nullableResinInfo = await ResinDatabaseInteractionHandler.GetResinForUserAsync(discordId);
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
