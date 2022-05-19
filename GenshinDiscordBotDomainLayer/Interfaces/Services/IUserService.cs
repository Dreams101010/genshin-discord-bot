using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services
{
    public interface IUserService
    {
        public Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId);
        public Task CreateUserIfNotExistsAsync(ulong discordId);
        public Task SetUserLocale(ulong discordId, UserLocale newLocale);
        public Task SetRemindersStateAsync(ulong discordId, bool state);
    }
}
