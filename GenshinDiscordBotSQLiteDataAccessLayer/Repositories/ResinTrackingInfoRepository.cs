using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using Dapper;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    public class ResinTrackingInfoRepository
    {
        SQLiteConnectionProvider ConnectionProvider { get; set; }

        public ResinTrackingInfoRepository(SQLiteConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task AddOrUpdateResinCountAsync(ResinTrackingInfoDataModel resinTrackingInfo)
        {
            var conn = ConnectionProvider.GetConnection();
            var insertSql = @"INSERT OR IGNORE INTO resin_tracking 
                        (user_discord_id, init_time, resin_count) 
                        VALUES (@UserDiscordId, @StartTime, @StartCount)";
            var updateSql = @"UPDATE resin_tracking SET 
                            init_time = @StartTime,
                            resin_count = @StartCount WHERE user_discord_id = @UserDiscordId;";
            int affectedByInsert = await conn.ExecuteAsync(insertSql, resinTrackingInfo);
            if (affectedByInsert != 0)
            {
                int affectedByUpdate = await conn.ExecuteAsync(updateSql, resinTrackingInfo);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public async Task<ResinTrackingInfoDataModel?> GetResinTrackingInfoByDiscordIdAsync(ulong id)
        {
            var conn = ConnectionProvider.GetConnection();
            var selectSql = @"SELECT 
                    user_discord_id UserDiscordId, init_time StartTime, resin_count StartCount 
                    FROM resin_tracking WHERE UserDiscordId = @UserDiscordId;";
            ResinTrackingInfoDataModel? user 
                = await conn.QueryFirstOrDefaultAsync<ResinTrackingInfoDataModel>(
                selectSql, new { UserDiscordID = id });
            return user;
        }
    }
}
