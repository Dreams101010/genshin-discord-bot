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
    public class ResinTrackingInfoRepository : IResinTrackingInfoRepository
    {
        public SqliteConnection Connection { get; }

        public ResinTrackingInfoRepository(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task AddOrUpdateResinCountAsync(ResinTrackingInfo resinTrackingInfo)
        {
            var insertSql = @"INSERT OR IGNORE INTO resin_tracking 
                        (user_discord_id, init_time, resin_count) 
                        VALUES (@UserDiscordId, @StartTime, @StartCount)";
            var updateSql = @"UPDATE resin_tracking SET 
                            init_time = @StartTime,
                            resin_count = @StartCount WHERE user_discord_id = @UserDiscordId;";
            var resinTrackingInfoDataModel = new ResinTrackingInfoDataModel(resinTrackingInfo);
            int affectedByInsert = await Connection.ExecuteAsync(insertSql, resinTrackingInfoDataModel);
            if (affectedByInsert == 0)
            {
                int affectedByUpdate = await Connection.ExecuteAsync(updateSql, resinTrackingInfoDataModel);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public async Task<ResinTrackingInfo?> GetResinTrackingInfoByDiscordIdAsync(ulong id)
        {
            var selectSql = @"SELECT 
                    user_discord_id UserDiscordId, init_time StartTime, resin_count StartCount 
                    FROM resin_tracking WHERE UserDiscordId = @UserDiscordId;";
            ResinTrackingInfoDataModel resinTrackingInfo
                = await Connection.QueryFirstOrDefaultAsync<ResinTrackingInfoDataModel>(
                selectSql, new { UserDiscordId = id });
            if (resinTrackingInfo.Equals(default(ResinTrackingInfoDataModel)))
            {
                return null;
            }
            return resinTrackingInfo.ToResinTrackingInfo();
        }
    }
}
