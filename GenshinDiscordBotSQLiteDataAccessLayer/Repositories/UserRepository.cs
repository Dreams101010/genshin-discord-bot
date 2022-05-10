using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using Dapper;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SQLiteConnectionProvider ConnectionProvider { get; }

        public UserRepository(SQLiteConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider 
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public void InsertOrUpdateUser(User user)
        {
            var conn = ConnectionProvider.GetConnection();
            var insertSql = @"INSERT OR IGNORE INTO users 
                        (discord_user_id, user_locale) 
                        VALUES (@DiscordId, @Locale)";
            var updateSql = @"UPDATE users SET user_locale = @Locale
                            WHERE discord_user_id = @DiscordId";
            var userDataModel = new UserDataModel(user);
            int affectedByInsert = conn.Execute(insertSql, userDataModel);
            if (affectedByInsert == 0)
            {
                int affectedByUpdate = conn.Execute(updateSql, userDataModel);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public User? GetUserByDiscordId(ulong id)
        {
            var conn = ConnectionProvider.GetConnection();
            var selectSql = @"SELECT 
                discord_user_id DiscordId, user_locale Locale
                FROM users WHERE discord_user_id = @DiscordId";
            UserDataModel user = conn.QueryFirstOrDefault<UserDataModel>(
                selectSql, new { DiscordId = id });
            if (user.Equals(default(UserDataModel)))
            {
                return null;
            }
            return user.ToUserDomain();
        }
    }
}
