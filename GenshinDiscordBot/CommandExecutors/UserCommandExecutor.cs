using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class UserCommandExecutor
    {
        private ILogger Logger { get; }
        private IUserService UserService { get; }
        private UserHelper UserHelper { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private UserResponseGenerator UserResponseGenerator { get; }

        public UserCommandExecutor(
            ILogger logger,
            IUserService userService,
            UserHelper userHelper,
            GeneralResponseGenerator generalResponseGenerator,
            UserResponseGenerator userResponseGenerator) : base()
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            UserHelper = userHelper 
                ?? throw new ArgumentNullException(nameof(userHelper));
            GeneralResponseGenerator = generalResponseGenerator ?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            UserResponseGenerator = userResponseGenerator 
                ?? throw new ArgumentNullException(nameof(userResponseGenerator));
        }

        public async Task<string> GetHelpMessageAsync(ulong userDiscordId, string userName)
        {
            try
            {
                var id = userDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                string response = GeneralResponseGenerator.GetHelpMessage(userLocale);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> ListSettingsAsync(ulong userDiscordId, string userName)
        {
            try
            {
                var id = userDiscordId;
                var user = await UserService.ReadUserAndCreateIfNotExistsAsync(id);
                string response = UserResponseGenerator.GetUserSettingsList(user, userName);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> ListLanguagesAsync(ulong userDiscordId, string userName)
        {
            var id = userDiscordId;
            var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
            string response = UserResponseGenerator.GetListOfPossibleLanguages(userLocale, userName);
            return response;
        }

        public async Task<string> SetLanguageAsync(ulong userDiscordId, string localeToSet, string userName)
        {
            try
            {
                var id = userDiscordId;
                var originalUserLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                if (!UserHelper.IsLocaleOrLanguage(localeToSet))
                {
                    string errorMessage = UserResponseGenerator.GetLanguageErrorMessage(
                        originalUserLocale, userName);
                    return errorMessage;
                }
                var newLocale = UserHelper.GetLocaleFromString(localeToSet);
                await UserService.SetUserLocale(id, newLocale);
                string response = UserResponseGenerator.GetLanguageSuccessMessage(newLocale, userName);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> EnableRemindersAsync(ulong userDiscordId, string userName)
        {
            try
            {
                var id = userDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await UserService.SetRemindersStateAsync(id, state: true);
                string response = UserResponseGenerator.GetEnableRemindersSuccessMessage(userLocale, userName);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> DisableRemindersAsync(ulong userDiscordId, string userName)
        {
            try
            {
                var id = userDiscordId;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
                await UserService.SetRemindersStateAsync(id, state: false);
                string response = UserResponseGenerator.GetDisableRemindersSuccessMessage(userLocale, userName);
                return response;
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
