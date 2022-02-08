using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DatabaseFacades;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.CommandFacades
{
    public class UserFacade
    {
        private UserDatabaseFacade UserDatabaseFacade { get; }
        public UserFacade(UserDatabaseFacade userDatabaseFacade)
        {
            UserDatabaseFacade = userDatabaseFacade ?? throw new ArgumentNullException(nameof(userDatabaseFacade));
        }

        public async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            return await UserDatabaseFacade.ReadUserAndCreateIfNotExistsAsync(discordId);
        }

        public async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            await UserDatabaseFacade.CreateUserIfNotExistsAsync(discordId);
        }

        public async Task SetUserLocaleAsync(ulong discordId, UserLocale newLocale)
        {
            await UserDatabaseFacade.SetUserLocaleAsync(discordId, newLocale);
        }

        public async Task SetUserLocationAsync(ulong discordId, string newLocation)
        {
            await UserDatabaseFacade.SetUserLocationAsync(discordId, newLocation);
        }
    }
}
