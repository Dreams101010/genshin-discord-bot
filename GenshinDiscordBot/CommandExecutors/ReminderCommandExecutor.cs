using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Helpers;
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
        public ReminderConversionHelper ReminderConversionHelper { get; }
        private IUserService UserService { get; set; }

        public ReminderCommandExecutor(IReminderService reminderService, ILogger logger, 
            GeneralResponseGenerator generalResponseGenerator,
            ReminderResponseGenerator reminderResponseGenerator,
            ReminderConversionHelper reminderConversionHelper,
            IUserService userService)
        {
            ReminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator ?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ReminderResponseGenerator = reminderResponseGenerator ?? throw new ArgumentNullException(nameof(reminderResponseGenerator));
            ReminderConversionHelper = reminderConversionHelper ?? throw new ArgumentNullException(nameof(reminderConversionHelper));
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

        public async Task<string> UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(DiscordMessageContext messageContext)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(messageContext);
                return ReminderResponseGenerator.GetSereniteaPotPlantHarvestSetupSuccessMessage(userLocale);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveSereniteaPotPlantHarvestRemindersForUserAsync(DiscordMessageContext messageContext)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveSereniteaPotPlantHarvestRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator.GetSereniteaPotPlantHarvestCancelSuccessMessage(userLocale);
                }
                else
                {
                    return ReminderResponseGenerator.GetSereniteaPotPlantHarvestCheckInReminderCancelNotFoundMessage(userLocale);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> GetRemindersForUserAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var reminderList = await ReminderService.GetRemindersForUserAsync(id);
                var reminderResultModelList = ReminderConversionHelper.GetReminderResultModelList(reminderList);
                var result = ReminderResponseGenerator.GetReminderListString(userLocale, reminderResultModelList);
                return result;
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
