using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IReminderRepository
    {
        public Task UpdateOrInsertReminderAsync(ReminderInsertModel reminderInfo);
        public Task<bool> RemoveRemindersForUserAsync(ReminderRemoveModel reminderInfo);
    }
}
