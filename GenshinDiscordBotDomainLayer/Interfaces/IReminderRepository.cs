using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IReminderRepository
    {
        public Task UpdateOrInsertReminderAsync(ReminderInsertModel reminderInfo);
        public Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo);
        public Task<List<Reminder>> GetRemindersPastTimeAsync(ulong timeInSeconds);
        public Task UpdateExpiredRecurrentRemindersAsync(ulong timeInSeconds);
        public Task RemoveExpiredNonRecurrentRemindersAsync(ulong currentTimeInSeconds);
    }
}
