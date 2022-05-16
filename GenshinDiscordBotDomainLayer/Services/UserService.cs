using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class UserService : IUserService
    {
        private IUserDatabaseInteractionHandler UserDatabaseInteractionHandler { get; }

        public UserService(
            IUserDatabaseInteractionHandler userDatabaseInteractionHandler)
        {
            UserDatabaseInteractionHandler = userDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(userDatabaseInteractionHandler));
        }

        public virtual async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            return await UserDatabaseInteractionHandler.ReadUserAndCreateIfNotExistsAsync(discordId);
        }

        public virtual async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            await UserDatabaseInteractionHandler.CreateUserIfNotExistsAsync(discordId);
        }

        public virtual async Task SetUserLocale(ulong discordId, UserLocale newLocale)
        {
            await UserDatabaseInteractionHandler.SetUserLocaleAsync(discordId, newLocale);
        }
    }
}
