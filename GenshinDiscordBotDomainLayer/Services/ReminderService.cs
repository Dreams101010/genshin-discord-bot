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
                ReminderTime = DateTimeBusinessLogic.GetCurrentUtcTimeAsUnixSeconds(),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }
    }
}
