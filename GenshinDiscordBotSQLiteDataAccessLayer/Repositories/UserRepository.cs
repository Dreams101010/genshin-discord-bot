using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using Dapper;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    public class UserRepository
    {
        private SQLiteConnectionProvider ConnectionProvider { get; }

        public UserRepository(SQLiteConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider 
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task InsertOrUpdateUserAsync(UserDataModel user)
        {
            var conn = ConnectionProvider.GetConnection();
            var insertSql = @"INSERT OR IGNORE INTO users 
                        (discord_user_id, user_location, user_locale) 
                        VALUES (@DiscordId, @Location, @Locale)";
            var updateSql = @"UPDATE users SET user_location = @Location,
                              user_locale = @Locale
                            WHERE discord_user_id = @DiscordId";
            int affectedByInsert = await conn.ExecuteAsync(insertSql, user);
            if (affectedByInsert == 0)
            {
                int affectedByUpdate = await conn.ExecuteAsync(updateSql, user);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public async Task<UserDataModel?> GetUserByDiscordIdAsync(ulong id)
        {
            var conn = ConnectionProvider.GetConnection();
            var selectSql = @"SELECT 
                discord_user_id DiscordId, user_locale Locale, user_location Location 
                FROM users WHERE discord_user_id = @DiscordId";
            UserDataModel user = await conn.QueryFirstOrDefaultAsync<UserDataModel>(
                selectSql, new { DiscordId = id });
            if (user.Equals(default(UserDataModel)))
            {
                return null;
            }
            return user;
        }
    }
}
