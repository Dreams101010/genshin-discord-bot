using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotDomainLayer.Helpers.Time;
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

        public async Task<string> UpdateOrCreateArtifactReminderAsync(
            DiscordMessageContext messageContext, string userName)
        {
			try
			{
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateArtifactReminderAsync(messageContext);
                return ReminderResponseGenerator.GetArtifactReminderSetupSuccessMessage(userLocale, userName);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
				return errorMessage;
			}
		}

        public async Task<string> UpdateOrCreateArtifactReminderWithCustomTimeAsync(
            DiscordMessageContext messageContext, string time, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var parseSuccess = TimeParser.TryParseTime(time, out Time timeObject);
                if (!parseSuccess)
                {
                    return ReminderResponseGenerator
                        .GetReminderTimeInvalid(userLocale, userName);
                }
                var timeOnly = new TimeOnly(timeObject.Hours, timeObject.Minutes);
                await ReminderService.UpdateOrCreateArtifactReminderWithCustomTimeAsync(messageContext, timeOnly);
                return ReminderResponseGenerator
                    .GetArtifactReminderSetupSuccessMessageWithCustomTime(userLocale, timeOnly, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveArtifactRemindersForUserAsync(
            DiscordMessageContext messageContext, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveArtifactRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetArtifactReminderCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetArtifactReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateCheckInReminderAsync(
            DiscordMessageContext messageContext, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateCheckInReminderAsync(messageContext);
                return ReminderResponseGenerator.GetCheckInReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateCheckInReminderWithCustomTimeAsync(
            DiscordMessageContext messageContext, string time, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var parseSuccess = TimeParser.TryParseTime(time, out Time timeObject);
                if (!parseSuccess)
                {
                    return ReminderResponseGenerator.GetReminderTimeInvalid(userLocale, userName);
                }
                var timeOnly = new TimeOnly(timeObject.Hours, timeObject.Minutes);
                await ReminderService.UpdateOrCreateCheckInReminderWithCustomTimeAsync(messageContext, timeOnly);
                return ReminderResponseGenerator
                    .GetCheckInReminderSetupSuccessMessageWithCustomTime(
                        userLocale, timeOnly, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveCheckInRemindersForUserAsync(
            DiscordMessageContext messageContext, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveCheckInRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetCheckInReminderCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetCheckInReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(
            DiscordMessageContext messageContext, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await ReminderService.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(messageContext);
                return ReminderResponseGenerator
                    .GetSereniteaPotPlantHarvestSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(
            DiscordMessageContext messageContext, int days, int hours, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                string validationErrorMessage = ReminderResponseGenerator
                    .GetUpdateOrCreateSereniteaPotPlantHarvestReminderValidationErrorMessage(
                    userLocale, days, hours, userName);
                if (validationErrorMessage != string.Empty)
                {
                    return validationErrorMessage;
                }
                await ReminderService.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(messageContext, days, hours);
                return ReminderResponseGenerator
                    .GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime(
                        userLocale, days, hours, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveSereniteaPotPlantHarvestRemindersForUserAsync(
            DiscordMessageContext messageContext, string userName)
        {
            try
            {
                var id = messageContext.UserDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var result = await ReminderService.RemoveSereniteaPotPlantHarvestRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetSereniteaPotPlantHarvestCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetSereniteaPotPlantHarvestCheckInReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> GetRemindersForUserAsync(ulong userDiscordId, string userName)
        {
            try
            {
                var id = userDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                var reminderList = await ReminderService.GetRemindersForUserAsync(id);
                var reminderResultModelList = ReminderConversionHelper.GetReminderResultModelList(reminderList);
                var result = ReminderResponseGenerator
                    .GetReminderListString(userLocale, reminderResultModelList, userName);
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
