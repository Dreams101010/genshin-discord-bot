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

        public virtual User ReadUserAndCreateIfNotExists(ulong discordId)
        {
            try
            {
                return UserDatabaseFacade.ReadUserAndCreateIfNotExists(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public virtual void CreateUserIfNotExistsAsync(ulong discordId)
        {
            try
            {
                UserDatabaseFacade.CreateUserIfNotExists(discordId);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }

        public virtual void SetUserLocale(ulong discordId, UserLocale newLocale)
        {
            try
            {
                UserDatabaseFacade.SetUserLocale(discordId, newLocale);
            }
            catch (Exception e)
            {
                ErrorHandler.LogException(e);
                throw;
            }
        }
    }
}
