using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using Dapper;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SqliteConnection Connection { get; }

        public UserRepository(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task InsertOrUpdateUserAsync(User user)
        {
            var insertSql = @"INSERT OR IGNORE INTO users 
                        (discord_user_id, user_locale, reminders_opt_in) 
                        VALUES (@DiscordId, @Locale, @RemindersOptIn)";
            var updateSql = @"UPDATE users SET user_locale = @Locale, 
                                               reminders_opt_in = @RemindersOptIn
                            WHERE discord_user_id = @DiscordId";
            var userDataModel = new UserDataModel(user);
            int affectedByInsert = await Connection.ExecuteAsync(insertSql, userDataModel);
            if (affectedByInsert == 0)
            {
                int affectedByUpdate = await Connection.ExecuteAsync(updateSql, userDataModel);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public async Task<User?> GetUserByDiscordIdAsync(ulong id)
        {
            var selectSql = @"SELECT 
                discord_user_id DiscordId, user_locale Locale, reminders_opt_in RemindersOptIn 
                FROM users WHERE discord_user_id = @DiscordId";
            UserDataModel user = await Connection.QueryFirstOrDefaultAsync<UserDataModel>(
                selectSql, new { DiscordId = id });
            if (user.Equals(default(UserDataModel)))
            {
                return null;
            }
            return user.ToUserDomain();
        }
    }
}
