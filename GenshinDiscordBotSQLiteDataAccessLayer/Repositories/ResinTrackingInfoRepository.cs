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
    public class ResinTrackingInfoRepository : IResinTrackingInfoRepository
    {
        private SQLiteConnectionProvider ConnectionProvider { get; }

        public ResinTrackingInfoRepository(SQLiteConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public void AddOrUpdateResinCount(ResinTrackingInfo resinTrackingInfo)
        {
            var conn = ConnectionProvider.GetConnection();
            var insertSql = @"INSERT OR IGNORE INTO resin_tracking 
                        (user_discord_id, init_time, resin_count) 
                        VALUES (@UserDiscordId, @StartTime, @StartCount)";
            var updateSql = @"UPDATE resin_tracking SET 
                            init_time = @StartTime,
                            resin_count = @StartCount WHERE user_discord_id = @UserDiscordId;";
            var resinTrackingInfoDataModel = new ResinTrackingInfoDataModel(resinTrackingInfo);
            int affectedByInsert = conn.Execute(insertSql, resinTrackingInfoDataModel);
            if (affectedByInsert == 0)
            {
                int affectedByUpdate = conn.Execute(updateSql, resinTrackingInfoDataModel);
                if (affectedByUpdate == 0)
                {
                    // TODO: create specific exceptions
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public ResinTrackingInfo? GetResinTrackingInfoByDiscordId(ulong id)
        {
            var conn = ConnectionProvider.GetConnection();
            var selectSql = @"SELECT 
                    user_discord_id UserDiscordId, init_time StartTime, resin_count StartCount 
                    FROM resin_tracking WHERE UserDiscordId = @UserDiscordId;";
            ResinTrackingInfoDataModel resinTrackingInfo
                = conn.QueryFirstOrDefault<ResinTrackingInfoDataModel>(
                selectSql, new { UserDiscordId = id });
            if (resinTrackingInfo.Equals(default(ResinTrackingInfoDataModel)))
            {
                return null;
            }
            return resinTrackingInfo.ToResinTrackingInfo();
        }
    }
}
