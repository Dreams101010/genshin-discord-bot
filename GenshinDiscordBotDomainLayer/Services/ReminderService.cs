using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task UpdateOrCreateReminderAsync(
            DiscordMessageContext messageContext, string description, TimeSpan timeSpan)
        {
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Generic reminder",
                Message = description,
                Interval = (ulong)timeSpan.TotalSeconds,
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(timeSpan),
            };
            await ReminderDatabaseInteractionHandler.CreateNewReminderAsync(reminderInfo);
        }

        public async Task UpdateOrCreateRecurrentReminderAsync
            (DiscordMessageContext messageContext, string description, TimeSpan timeSpan)
        {
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Generic reminder",
                Message = description,
                Interval = (ulong)timeSpan.TotalSeconds,
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(timeSpan),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.CreateNewReminderAsync(reminderInfo);
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
                Interval = DateTimeBusinessLogic.GetSecondsInHours(24),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(24, 0, 0)),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task UpdateOrCreateArtifactReminderWithCustomTimeAsync
            (DiscordMessageContext messageContext, TimeOnly timeOnly)
        {
            var message = await ReminderMessageBusinessLogic.GetArtifactReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Artifact reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours(24),
                ReminderTime = DateTimeBusinessLogic.GetUtcTimeToNextDailyReminderAsUnixSeconds(timeOnly),
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

        public async Task UpdateOrCreateCheckInReminderAsync(DiscordMessageContext messageContext)
        {
            var message = await ReminderMessageBusinessLogic.GetCheckInReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Check-in reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours(24),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(24, 0, 0)),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task UpdateOrCreateCheckInReminderWithCustomTimeAsync
            (DiscordMessageContext messageContext, TimeOnly timeOnly)
        {
            var message = await ReminderMessageBusinessLogic.GetCheckInReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Check-in reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours(24),
                ReminderTime = DateTimeBusinessLogic.GetUtcTimeToNextDailyReminderAsUnixSeconds(timeOnly),
                Recurrent = true,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveCheckInRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            ReminderRemoveModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                CategoryName = "Check-in reminder",
            };
            return await ReminderDatabaseInteractionHandler.RemoveRemindersForUserAsync(reminderInfo);
        }

        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(DiscordMessageContext messageContext)
        {
            var message = await ReminderMessageBusinessLogic.GetSereniteaPotPlantHarvestReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Serenitea pot plant harvest",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours(68),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(2, 22, 0, 0)),
                Recurrent = false,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(DiscordMessageContext messageContext, int days, int hours)
        {
            var message = await ReminderMessageBusinessLogic.GetSereniteaPotPlantHarvestReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Serenitea pot plant harvest",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours((uint)(days * 24 + hours)),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(days, hours, 0, 0)),
                Recurrent = false,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveSereniteaPotPlantHarvestRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            ReminderRemoveModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                CategoryName = "Serenitea pot plant harvest",
            };
            return await ReminderDatabaseInteractionHandler.RemoveRemindersForUserAsync(reminderInfo);
        }

        public async Task UpdateOrCreateParametricTransformerReminderAsync(DiscordMessageContext messageContext)
        {
            var message = await ReminderMessageBusinessLogic.GetParametricTransformerReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Parametric Transformer reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours(7 * 24),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(7, 0, 0, 0)),
                Recurrent = false,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task UpdateOrCreateParametricTransformerReminderAsync(DiscordMessageContext messageContext, int days, int hours)
        {
            var message = await ReminderMessageBusinessLogic.GetParametricTransformerReminderMessage(messageContext.UserDiscordId);
            ReminderInsertModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                ChannelId = messageContext.ChannelId,
                GuildId = messageContext.GuildId,
                CategoryName = "Parametric Transformer reminder",
                Message = message,
                Interval = DateTimeBusinessLogic.GetSecondsInHours((uint)(days * 24 + hours)),
                ReminderTime = DateTimeBusinessLogic.GetReminderUtcTimeAsUnixSeconds(new TimeSpan(days, hours, 0, 0)),
                Recurrent = false,
            };
            await ReminderDatabaseInteractionHandler.UpdateOrCreateReminderAsync(reminderInfo);
        }

        public async Task<bool> RemoveParametricTransformerRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            ReminderRemoveModel reminderInfo = new()
            {
                UserDiscordId = messageContext.UserDiscordId,
                CategoryName = "Parametric Transformer reminder",
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

        public async Task<List<Reminder>> GetRemindersForUserAsync(ulong userDiscordId)
        {
            return await ReminderDatabaseInteractionHandler.GetRemindersForUserAsync(userDiscordId);
        }

        public async Task<bool> RemoveReminderByIdAsync(ulong requesterDiscordId, ulong reminderId)
        {
            return await ReminderDatabaseInteractionHandler.RemoveReminderByIdAsync(requesterDiscordId, reminderId);
        }
    }
}
