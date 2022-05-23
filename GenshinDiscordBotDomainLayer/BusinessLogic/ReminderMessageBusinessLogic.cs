using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;

namespace GenshinDiscordBotDomainLayer.BusinessLogic
{
    public class ReminderMessageBusinessLogic
    {
        private IUserService UserService { get; set; }

        public ReminderMessageBusinessLogic(IUserService userService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<string> GetArtifactReminderMessage(ulong discordUserId)
        {
            var user = await UserService.ReadUserAndCreateIfNotExistsAsync(discordUserId);
            return user.Locale switch
            {
                DomainModels.UserLocale.enGB => "Time to collect artifacts! :)",
                DomainModels.UserLocale.ruRU => "Время собирать артефакты! :)",
                _ => throw new NotImplementedException("Invalid enum state"),
            };
        }

        public async Task<string> GetCheckInReminderMessage(ulong discordUserId)
        {
            var user = await UserService.ReadUserAndCreateIfNotExistsAsync(discordUserId);
            return user.Locale switch
            {
                DomainModels.UserLocale.enGB => "Time to check-in on hoyolab.com! :)",
                DomainModels.UserLocale.ruRU => "Время ежедневной отметки на hoyolab.com! :)",
                _ => throw new NotImplementedException("Invalid enum state"),
            };
        }
    }
}
