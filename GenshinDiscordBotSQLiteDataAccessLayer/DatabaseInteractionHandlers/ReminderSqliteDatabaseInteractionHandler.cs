using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public class ReminderSqliteDatabaseInteractionHandler : SqliteDatabaseInteractionHandler, IReminderDatabaseInteractionHandler
    {
        private IReminderRepository ReminderRepository { get; }

        public ReminderSqliteDatabaseInteractionHandler(
            IReminderRepository reminderRepository, SqliteConnection connection)
            : base(connection)
        {
            ReminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
        }
        public async Task UpdateOrCreateReminderAsync(ReminderInsertModel reminderInfo)
        {
            await ExecuteInTransactionAsync(async () => await UpdateOrCreateReminderFuncAsync(reminderInfo));
        }

        private async Task UpdateOrCreateReminderFuncAsync(ReminderInsertModel reminderInfo)
        {
            await ReminderRepository.UpdateOrInsertReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo)
        {
            return await ExecuteInTransactionAsync(async () => await RemoveRemindersForUserFuncAsync(reminderInfo));
        }

        private async Task<bool> RemoveRemindersForUserFuncAsync(ReminderRemoveModel reminderInfo)
        {
            return await ReminderRepository.RemoveRemindersForUserAsync(reminderInfo);
        }
    }
}
