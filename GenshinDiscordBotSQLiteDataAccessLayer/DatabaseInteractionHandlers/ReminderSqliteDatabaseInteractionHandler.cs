using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public class ReminderSqliteDatabaseInteractionHandler
        : SqliteDatabaseInteractionHandler, IReminderDatabaseInteractionHandler
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

        public async Task CreateNewReminderAsync(ReminderInsertModel reminderInfo)
        {
            await ExecuteInTransactionAsync(async () => await CreateNewReminderFuncAsync(reminderInfo));
        }

        public async Task CreateNewReminderFuncAsync(ReminderInsertModel reminderInfo)
        {
            await ReminderRepository.CreateNewReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo)
        {
            return await ExecuteInTransactionAsync(async () => await RemoveRemindersForUserFuncAsync(reminderInfo));
        }

        private async Task<bool> RemoveRemindersForUserFuncAsync(ReminderRemoveModel reminderInfo)
        {
            return await ReminderRepository.RemoveRemindersForUserAsync(reminderInfo);
        }

        public async Task<List<Reminder>> GetRemindersPastTimeAsync(ulong time)
        {
            return await ExecuteInTransactionAsync(async () => await GetRemindersPastTimeFuncAsync(time));
        }

        private async Task<List<Reminder>> GetRemindersPastTimeFuncAsync(ulong time)
        {
            return await ReminderRepository.GetRemindersPastTimeAsync(time);
        }

        public async Task UpdateExpiredRecurrentRemindersAsync(ulong timeInSeconds)
        {
            await ExecuteInTransactionAsync(async () => await UpdateExpiredRecurrentRemindersFuncAsync(timeInSeconds));
        }

        private async Task UpdateExpiredRecurrentRemindersFuncAsync(ulong timeInSeconds)
        {
            await ReminderRepository.UpdateExpiredRecurrentRemindersAsync(timeInSeconds);
        }

        public async Task RemoveExpiredNonRecurrentRemindersAsync(ulong currentTimeInSeconds)
        {
            await ExecuteInTransactionAsync(async () => await RemoveExpiredNonRecurrentRemindersFuncAsync(currentTimeInSeconds));
        }

        public async Task RemoveExpiredNonRecurrentRemindersFuncAsync(ulong currentTimeInSeconds)
        {
            await ReminderRepository.RemoveExpiredNonRecurrentRemindersAsync(currentTimeInSeconds);
        }

        public async Task<List<Reminder>> GetRemindersForUserAsync(ulong userDiscordId)
        {
            return await ExecuteInTransactionAsync(async () => await GetRemindersForUserFuncAsync(userDiscordId));
        }

        private async Task<List<Reminder>> GetRemindersForUserFuncAsync(ulong userDiscordId)
        {
            return await ReminderRepository.GetRemindersForUserAsync(userDiscordId);
        }

        public async Task<bool> RemoveReminderByIdAsync(ulong requesterDiscordId, ulong reminderId)
        {
            return await ExecuteInTransactionAsync(async () 
                => await RemoveReminderByIdFuncAsync(requesterDiscordId, reminderId));
        }

        private async Task<bool> RemoveReminderByIdFuncAsync(ulong requesterDiscordId, ulong reminderId)
        {
            return await ReminderRepository.RemoveReminderById(requesterDiscordId, reminderId);
        }
    }
}
