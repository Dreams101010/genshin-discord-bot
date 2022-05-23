using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class ReminderService : IReminderService
    {
        private ReminderMessageBusinessLogic ReminderMessageBusinessLogic { get; set; }
        private DateTimeBusinessLogic DateTimeBusinessLogic { get; set; }
        public IReminderDatabaseInteractionHandler ReminderDatabaseInteractionHandler { get; }

        public ReminderService(ReminderMessageBusinessLogic reminderMessageBusinessLogic,
            DateTimeBusinessLogic dateTimeBusinessLogic,
            IReminderDatabaseInteractionHandler reminderDatabaseInteractionHandler)
        {
            ReminderMessageBusinessLogic = reminderMessageBusinessLogic 
                ?? throw new ArgumentNullException(nameof(reminderMessageBusinessLogic));
            DateTimeBusinessLogic = dateTimeBusinessLogic 
                ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
            ReminderDatabaseInteractionHandler = reminderDatabaseInteractionHandler 
                ?? throw new ArgumentNullException(nameof(reminderDatabaseInteractionHandler));
        }

        public async Task UpdateOrCreateArtifactReminderAsync(DiscordMessageContext messageContext)
        {
            var message = await ReminderMessageBusinessLogic.GetArtifactReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Artifact reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetHoursAsTotalSeconds(24),
                ReminderTime = DateTimeBusinessLogic.GetReminderTimeAsUnixSeconds(new TimeSpan(24, 0, 0)),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveArtifactRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            ReminderRemoveModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                CategoryName = "Artifact reminder",
            };
            return await ReminderDatabaseInteractionHandler.RemoveRemindersForUserAsync(reminderInfo);
        }

        public async Task<List<Reminder>> GetExpiredRemindersAsync(ulong timeInSeconds)
        {
            var reminders = await ReminderDatabaseInteractionHandler
                .GetRemindersPastTimeAsync(timeInSeconds);
            return reminders;
        }

        public async Task UpdateExpiredRecurrentRemindersAsync(ulong timeInSeconds)
        {
            await ReminderDatabaseInteractionHandler.UpdateExpiredRecurrentRemindersAsync(timeInSeconds);
        }

        public async Task RemoveExpiredNonRecurrentRemindersAsync(ulong timeInSeconds)
        {
            await ReminderDatabaseInteractionHandler.RemoveExpiredNonRecurrentRemindersAsync(timeInSeconds);
        }
    }
}
