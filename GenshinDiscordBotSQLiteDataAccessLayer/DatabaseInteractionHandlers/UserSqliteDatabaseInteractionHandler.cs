using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public class UserSqliteDatabaseInteractionHandler : SqliteDatabaseInteractionHandler, IUserDatabaseInteractionHandler
    {
        private IUserRepository UserRepository { get; }

        public UserSqliteDatabaseInteractionHandler(IUserRepository userRepository, SqliteConnection connection)
            : base(connection)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> ReadUserAndCreateIfNotExistsAsync(ulong discordId)
        {
            return await ExecuteInTransactionAsync(async () => await ReadUserAndCreateIfNotExistsFuncAsync(discordId));
        }

        private async Task<User> ReadUserAndCreateIfNotExistsFuncAsync(ulong discordId)
        {
            // try get user
            User? nullableUser = await UserRepository.GetUserByDiscordIdAsync(discordId);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return nullableUser.GetValueOrDefault();
            }
            else
            {
                // if we don't have user create it
                await CreateDefaultUserWithIdAsync(discordId);
                nullableUser = await UserRepository.GetUserByDiscordIdAsync(discordId);
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    return nullableUser.GetValueOrDefault();
                }
                else
                {
                    throw new Exception("Database interaction exception: could not read created user");
                }
            }
        }

        public async Task CreateUserIfNotExistsAsync(ulong discordId)
        {
            await ExecuteInTransactionAsync(async () => await CreateUserIfNotExistsFuncAsync(discordId));
        }

        private async Task CreateUserIfNotExistsFuncAsync(ulong discordId)
        {
            // try get user
            User? nullableUser = await UserRepository.GetUserByDiscordIdAsync(discordId);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return;
            }
            else
            {
                // if we don't have user create it
                await CreateDefaultUserWithIdAsync(discordId);
                nullableUser = await UserRepository.GetUserByDiscordIdAsync(discordId);
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
            await ExecuteInTransactionAsync(async () => await SetUserLocaleFuncAsync(discordId, newLocale));
        }

        private async Task SetUserLocaleFuncAsync(ulong discordId, UserLocale newLocale)
        {
            var user = await ReadUserAndCreateIfNotExistsFuncAsync(discordId);
            user.Locale = newLocale;
            await AddOrUpdateUserAsync(user);
        }

        private async Task CreateDefaultUserWithIdAsync(ulong discordId)
        {
            User newUser = User.GetDefaultUser();
            newUser.DiscordId = discordId;
            await UserRepository.InsertOrUpdateUserAsync(newUser);
        }

        private async Task AddOrUpdateUserAsync(User user)
        {
            await UserRepository.InsertOrUpdateUserAsync(user);
        }
    }
}
