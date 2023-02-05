using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.ResultModels;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ValidationLogic;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class ResinService : IResinService
    {
        private IUserDatabaseInteractionHandler UserDatabaseInteractionHandler { get; }
        private IResinDatabaseInteractionHandler ResinDatabaseInteractionHandler { get; }
        private ResinBusinessLogic ResinBusinessLogic { get; }
        private ResinCommandArgumentValidator ResinValidator { get; }

        public ResinService(IUserDatabaseInteractionHandler userDatabaseInteractionHandler,
            IResinDatabaseInteractionHandler resinDatabaseInteractionHandler,
            ResinBusinessLogic resinBusinessLogic,
            ResinCommandArgumentValidator resinValidator)
        {
            UserDatabaseInteractionHandler = userDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(userDatabaseInteractionHandler));
            ResinDatabaseInteractionHandler = resinDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(resinDatabaseInteractionHandler));
            ResinBusinessLogic = resinBusinessLogic 
                ?? throw new ArgumentNullException(nameof(resinBusinessLogic));
            ResinValidator = resinValidator 
                ?? throw new ArgumentNullException(nameof(resinValidator));
        }

        public async virtual Task<bool> SetResinForUserAsync(ulong discordId, int resinCount)
        {
            ResinValidator.SetResinCount_Validate(resinCount);
            await UserDatabaseInteractionHandler.CreateUserIfNotExistsAsync(discordId);
            var model = ResinBusinessLogic.GetResinTrackingInfo(resinCount, discordId);
            await ResinDatabaseInteractionHandler.SetResinForUserAsync(model);
            return true;
        }

        public async virtual Task<ResinInfoResultModel> GetResinForUserAsync(ulong discordId)
        {
            var user = await UserDatabaseInteractionHandler.ReadUserAndCreateIfNotExistsAsync(discordId);
            var resinInfo = await ResinDatabaseInteractionHandler.GetResinForUserAsync(discordId);
            if (resinInfo.IsEmpty)
            {
                return ResinInfoResultModel.Empty;
            }
            var result = ResinBusinessLogic.GetResinResult(user, resinInfo);
            return result;
        }
    }
}
