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
using GenshinDiscordBotDomainLayer.DomainModels;

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
            if (affectedByUpdate == 0)
            {
                int affectedByInsert = await Connection.ExecuteAsync(insertSql, reminderInfo);
                if (affectedByInsert == 0)
                {
                    throw new Exception("Database error while updating rows: affected mismatch");
                }
            }
        }

        public async Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo)
        {
            string removeSql = @"
                DELETE FROM reminders 
                WHERE user_discord_id = @UserDiscordId
                AND category_id = (SELECT id FROM reminder_categories WHERE name = @CategoryName);
            ";
            int affectedCount = await Connection.ExecuteAsync(removeSql, reminderInfo);
            return affectedCount > 0;
        }

        public async Task<List<Reminder>> GetRemindersPastTimeAsync(ulong timeInSeconds)
        {
            string sql = @"
                SELECT users.discord_user_id UserDiscordId, user_locale UserLocale, 
                reminders_opt_in RemindersOptInFlag, guild_id GuildId, channel_id ChannelId, 
                interval Interval, reminder_time ReminderTime, name CategoryName, 
                message Message, recurrent RecurrentFlag
                FROM users 
                INNER JOIN reminders ON users.discord_user_id = reminders.user_discord_id 
                INNER JOIN reminder_categories ON reminders.category_id = reminder_categories.id
                WHERE reminder_time < @Time AND reminders_opt_in = true;
                ";
            return (await Connection.QueryAsync<ReminderDataModel>
                (sql, new { Time = timeInSeconds })).Select((x => x.ToReminderDomain())).ToList();
        }

        public async Task UpdateExpiredRecurrentRemindersAsync(ulong currentTimeInSeconds)
        {
            string sql = @"
                UPDATE reminders SET reminder_time = reminder_time + interval 
                WHERE reminder_time < @CurrentTime AND recurrent = true;
            ";
            await Connection.ExecuteAsync(sql, new { CurrentTime = currentTimeInSeconds });
        }

        public async Task RemoveExpiredNonRecurrentRemindersAsync(ulong currentTimeInSeconds)
        {
            string sql = @"
                DELETE FROM reminders WHERE reminder_time < @CurrentTime AND recurrent = false;
            ";
            await Connection.ExecuteAsync(sql, new { CurrentTime = currentTimeInSeconds });
        }

        public async Task<List<Reminder>> GetRemindersForUserAsync(ulong userDiscordId)
        {
            string sql = @"
                SELECT users.discord_user_id UserDiscordId, user_locale UserLocale, 
                reminders_opt_in RemindersOptInFlag, guild_id GuildId, channel_id ChannelId, 
                interval Interval, reminder_time ReminderTime, name CategoryName, 
                message Message, recurrent RecurrentFlag
                FROM users 
                INNER JOIN reminders ON users.discord_user_id = reminders.user_discord_id 
                INNER JOIN reminder_categories ON reminders.category_id = reminder_categories.id
                WHERE discord_user_id = @UserDiscordId;
            ";
            return (await Connection.QueryAsync<ReminderDataModel>
                    (sql, new { UserDiscordId = userDiscordId })).Select((x => x.ToReminderDomain())).ToList();
        }
    }
}
