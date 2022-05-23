using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers
{
    public interface IReminderDatabaseInteractionHandler
    {
        public Task UpdateOrCreateReminderAsync(ReminderInsertModel reminderInfo);
    }
}
