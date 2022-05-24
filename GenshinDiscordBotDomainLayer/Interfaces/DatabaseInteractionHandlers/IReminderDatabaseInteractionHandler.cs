using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers
{
    public interface IReminderDatabaseInteractionHandler
    {
        public Task UpdateOrCreateReminderAsync(ReminderInsertModel reminderInfo);
        public Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo);
        public Task<List<Reminder>> GetRemindersPastTimeAsync(ulong time);
        public Task UpdateExpiredRecurrentRemindersAsync(ulong timeInSeconds);
        public Task RemoveExpiredNonRecurrentRemindersAsync(ulong currentTimeInSeconds);
        public Task<List<Reminder>> GetRemindersForUserAsync(ulong userDiscordId);
    }
}
