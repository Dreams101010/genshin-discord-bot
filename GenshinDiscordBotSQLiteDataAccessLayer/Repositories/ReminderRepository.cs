using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using Dapper;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        public SqliteConnection Connection { get; }

        public ReminderRepository(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        public async Task UpdateOrInsertReminderAsync(ReminderInsertModel reminderInfo)
        {
            Console.WriteLine("before update");
            string updateSql = @"
                UPDATE reminders SET guild_id = @GuildId, channel_id = @ChannelId, 
                interval = @Interval, reminder_time = @ReminderTime, 
                message = @Message, 
                recurrent = @Recurrent 
                WHERE 
                user_discord_id = @UserDiscordId AND 
                category_id = (SELECT id FROM reminder_categories WHERE name = @CategoryName);
            ";
            string insertSql = @"
                INSERT INTO reminders (guild_id, channel_id, user_discord_id, 
                interval, reminder_time, category_id, message, recurrent) 
                VALUES 
                (@GuildId, @ChannelId, @UserDiscordId, @Interval, @ReminderTime, 
                (SELECT id FROM reminder_categories WHERE name = @CategoryName), 
                @Message, @Recurrent);
            ";
            int affectedByUpdate = await Connection.ExecuteAsync(updateSql, reminderInfo);
            Console.WriteLine(affectedByUpdate);
            if (affectedByUpdate == 0)
            {
                Console.WriteLine("insert");
                int affectedByInsert = await Connection.ExecuteAsync(insertSql, reminderInfo);
                if (affectedByInsert == 0)
                {
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
                Console.WriteLine(affectedByInsert);
            }
        }
    }
}
