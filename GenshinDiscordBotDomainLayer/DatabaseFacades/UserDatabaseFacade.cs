using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;

namespace GenshinDiscordBotDomainLayer.DatabaseFacades
{
    public class UserDatabaseFacade
    {
        private ICommand<AddOrUpdateUserCommandParam, bool> AddOrUpdateUserCommand { get; }
        private IQuery<GetUserByDiscordIdQueryParam, User?> GetUserByDiscordIdQuery { get; }
        public UserDatabaseFacade(
            ICommand<AddOrUpdateUserCommandParam, bool> addOrUpdateUserCommand,
            IQuery<GetUserByDiscordIdQueryParam, User?> getUserByIdQuery)
        {
            AddOrUpdateUserCommand = addOrUpdateUserCommand 
                ?? throw new ArgumentNullException(nameof(addOrUpdateUserCommand));
            GetUserByDiscordIdQuery = getUserByIdQuery 
                ?? throw new ArgumentNullException(nameof(getUserByIdQuery));
        }

        public async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            // try get user
            var param = new GetUserByDiscordIdQueryParam
            {
                DiscordId = discordId
            };
            User? nullableUser = await GetUserByDiscordIdQuery.QueryAsync(param, true);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return nullableUser.Value;
            }
            else
            {
                // if we don't have user create it
                await CreateDefaultUserWithIdAsync(discordId);
                nullableUser = await GetUserByDiscordIdQuery.QueryAsync(param, true);
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    return nullableUser.Value;
                }
                else
                {
                    throw new Exception("Database interaction exception: could not read created user");
                }
            }
        }

        public async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            // try get user
            var param = new GetUserByDiscordIdQueryParam
            {
                DiscordId = discordId
            };
            User? nullableUser = await GetUserByDiscordIdQuery.QueryAsync(param, true);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return;
            }
            else
            {
                // if we don't have user create it
                await CreateDefaultUserWithIdAsync(discordId);
                nullableUser = await GetUserByDiscordIdQuery.QueryAsync(param, true);
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    return;
                }
                else
                {
                    throw new Exception("Database interaction exception: could not read created user");
                }
            }
        }

        public async Task SetUserLocaleAsync(ulong discordId, UserLocale newLocale)
        {
            var user = await ReadUserAndCreateIfNotExistsAsync(discordId);
            user.Locale = newLocale;
            await AddOrUpdateUserAsync(user);
        }

        public async Task SetUserLocationAsync(ulong discordId, string newLocation)
        {
            var user = await ReadUserAndCreateIfNotExistsAsync(discordId);
            user.Location = newLocation;
            await AddOrUpdateUserAsync(user);
        }

        private async Task CreateDefaultUserWithIdAsync(ulong discordId)
        {
            User newUser = User.GetDefaultUser();
            newUser.DiscordId = discordId;
            await AddOrUpdateUserAsync(newUser);
        }

        private async Task AddOrUpdateUserAsync(User user)
        {
            var param = new AddOrUpdateUserCommandParam
            {
                User = user
            };
            await AddOrUpdateUserCommand.ExecuteAsync(param);
        }
    }
}
