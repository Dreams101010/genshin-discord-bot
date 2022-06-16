using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services
{
    public interface IReminderService
    {
        public Task UpdateOrCreateArtifactReminderAsync(DiscordMessageContext messageContext);
        public Task UpdateOrCreateArtifactReminderWithCustomTimeAsync
            (DiscordMessageContext messageContext, TimeOnly timeOnly);
        public Task<bool> RemoveArtifactRemindersForUserAsync(DiscordMessageContext messageContext);
        public Task UpdateOrCreateCheckInReminderAsync(DiscordMessageContext messageContext);
        public Task UpdateOrCreateCheckInReminderWithCustomTimeAsync
            (DiscordMessageContext messageContext, TimeOnly timeOnly);
        public Task<bool> RemoveCheckInRemindersForUserAsync(DiscordMessageContext messageContext);
        public Task UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(DiscordMessageContext messageContext);
        public Task UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(DiscordMessageContext messageContext, int days, int hours);
        public Task<bool> RemoveSereniteaPotPlantHarvestRemindersForUserAsync(DiscordMessageContext messageContext);
        public Task<List<Reminder>> GetExpiredRemindersAsync(ulong timeInSeconds);
        public Task UpdateExpiredRecurrentRemindersAsync(ulong timeInSeconds);
        public Task RemoveExpiredNonRecurrentRemindersAsync(ulong currentTimeInSeconds);
        public Task<List<Reminder>> GetRemindersForUserAsync(ulong userDiscordId);
    }
}
