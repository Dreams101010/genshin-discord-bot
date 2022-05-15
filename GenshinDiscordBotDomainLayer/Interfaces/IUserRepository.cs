using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IUserRepository
    {
        public Task InsertOrUpdateUserAsync(User user);
        public Task<User?> GetUserByDiscordIdAsync(ulong id);
    }
}
