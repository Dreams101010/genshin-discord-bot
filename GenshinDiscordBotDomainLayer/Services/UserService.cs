using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DatabaseFacades;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ErrorHandlers;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class UserService
    {
        private UserDatabaseFacade UserDatabaseFacade { get; }
        private FacadeErrorHandler ErrorHandler { get; }

        public UserService(
            UserDatabaseFacade userDatabaseFacade, 
            FacadeErrorHandler errorHandler)
        {
            UserDatabaseFacade = userDatabaseFacade ?? throw new ArgumentNullException(nameof(userDatabaseFacade));
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            try
            {
                return await UserDatabaseFacade.ReadUserAndCreateIfNotExistsAsync(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            try
            {
                await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public async Task SetUserLocaleAsync(ulong discordId, UserLocale newLocale)
        {
            try
            {
                await UserDatabaseFacade.SetUserLocaleAsync(discordId, newLocale);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }
    }
}
