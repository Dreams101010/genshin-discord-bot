using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class ReminderCommandExecutor
    {
        private IReminderService ReminderService { get; set; }
        private ILogger Logger { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; set; }
        private ReminderResponseGenerator ReminderResponseGenerator { get; set; }
        private IUserService UserService { get; set; }

        public ReminderCommandExecutor(IReminderService reminderService, ILogger logger, 
            GeneralResponseGenerator generalResponseGenerator,
            ReminderResponseGenerator reminderResponseGenerator,
            IUserService userService)
        {
            ReminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator ?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ReminderResponseGenerator = reminderResponseGenerator ?? throw new ArgumentNullException(nameof(reminderResponseGenerator));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<string> UpdateOrCreateArtifactReminderAsync(DiscordMessageContext messageContext)
        {
			try
			{
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateArtifactReminderAsync(messageContext);
                return ReminderResponseGenerator.GetArtifactReminderSetupSuccessMessage(userLocale);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
				return errorMessage;
			}
		}

        public async Task<string> RemoveArtifactRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveArtifactRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator.GetArtifactReminderCancelSuccessMessage(userLocale);
                }
                else
                {
                    return ReminderResponseGenerator.GetArtifactReminderCancelNotFoundMessage(userLocale);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateCheckInReminderAsync(DiscordMessageContext messageContext)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateCheckInReminderAsync(messageContext);
                return ReminderResponseGenerator.GetCheckInReminderSetupSuccessMessage(userLocale);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveCheckInRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveCheckInRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator.GetCheckInReminderCancelSuccessMessage(userLocale);
                }
                else
                {
                    return ReminderResponseGenerator.GetCheckInReminderCancelNotFoundMessage(userLocale);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }
    }
}
