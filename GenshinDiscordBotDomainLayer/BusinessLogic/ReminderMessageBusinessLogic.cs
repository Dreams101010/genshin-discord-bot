using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class ReminderMessageBusinessLogic
    {
        private IUserService UserService { get; set; }
        public Localization.Localization Localization { get; }

        public ReminderMessageBusinessLogic(IUserService userService, 
            Localization.Localization localization)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public async Task<string> GetArtifactReminderMessage(ulong discordUserId)
        {
            var user = await UserService.ReadUserAndCreateIfNotExistsAsync(discordUserId);
            return user.Locale switch
            {
                DomainModels.UserLocale.enGB => Localization.English["Reminder"]["ArtifactReminderMessage"],
                DomainModels.UserLocale.ruRU => Localization.Russian["Reminder"]["ArtifactReminderMessage"],
                _ => throw new NotImplementedException("Invalid enum state"),
            };
        }

        public async Task<string> GetCheckInReminderMessage(ulong discordUserId)
        {
            var user = await UserService.ReadUserAndCreateIfNotExistsAsync(discordUserId);
            return user.Locale switch
            {
                DomainModels.UserLocale.enGB => Localization.English["Reminder"]["CheckInReminderMessage"],
                DomainModels.UserLocale.ruRU => Localization.Russian["Reminder"]["CheckInReminderMessage"],
                _ => throw new NotImplementedException("Invalid enum state"),
            };
        }

        public async Task<string> GetSereniteaPotPlantHarvestReminderMessage(ulong discordUserId)
        {
            var user = await UserService.ReadUserAndCreateIfNotExistsAsync(discordUserId);
            return user.Locale switch
            {
                DomainModels.UserLocale.enGB => Localization.English["Reminder"]["SereniteaPotPlantHarvestReminderMessage"],
                DomainModels.UserLocale.ruRU => Localization.Russian["Reminder"]["SereniteaPotPlantHarvestReminderMessage"],
                _ => throw new NotImplementedException("Invalid enum state"),
            };
        }
    }
}
