using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;

namespace GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers
{
    public interface IUserDatabaseInteractionHandler
    {
        public Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId);
        public Task CreateUserIfNotExistsAsync(ulong discordId);
        public Task SetUserLocaleAsync(ulong discordId, UserLocale newLocale);
    }
}
