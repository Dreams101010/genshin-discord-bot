using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;

namespace GenshinDiscordBotDomainLayer.Facades
{
    public class UserFacade
    {
        ICommand<AddOrUpdateUserCommandParam, bool> AddOrUpdateUserCommand { get; set; }
        IQuery<GetUserByDiscordIdQueryParam, User?> GetUserByDiscordIdQuery { get; set; }
        public UserFacade(
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
                Console.WriteLine("Creating user...");
                // if we don't have user create it
                await CreateDefaultUserWithIdAsync(discordId);
                Console.WriteLine("Created user.");
                nullableUser = await GetUserByDiscordIdQuery.QueryAsync(param, true);
                Console.WriteLine("Got user.");
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    Console.WriteLine("Returning user.");
                    return nullableUser.Value;
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
            user.Locale = new UserLocale();
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
