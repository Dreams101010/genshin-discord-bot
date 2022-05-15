using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ErrorHandlers;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class UserService
    {
        private IUserDatabaseInteractionHandler UserDatabaseInteractionHandler { get; }
        private FacadeErrorHandler ErrorHandler { get; }

        public UserService(
            IUserDatabaseInteractionHandler userDatabaseInteractionHandler,
            FacadeErrorHandler errorHandler)
        {
            UserDatabaseInteractionHandler = userDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(userDatabaseInteractionHandler));
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public virtual async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            try
            {
                return await UserDatabaseInteractionHandler.ReadUserAndCreateIfNotExistsAsync(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public virtual async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            try
            {
                await UserDatabaseInteractionHandler.CreateUserIfNotExistsAsync(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public virtual async Task SetUserLocale(ulong discordId, UserLocale newLocale)
        {
            try
            {
                await UserDatabaseInteractionHandler.SetUserLocaleAsync(discordId, newLocale);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }
    }
}
