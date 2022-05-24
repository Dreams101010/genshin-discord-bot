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

        public async Task<string> GetHelpMessageAsync()
        {
            try
            {
                string response = GeneralResponseGenerator.GetHelpMessage();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> ListSettingsAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                var user = await UserService.ReadUserAndCreateIfNotExistsAsync(id);
                string response = UserResponseGenerator.GetUserSettingsList(user);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public string ListLanguages()
        {
            string response = UserResponseGenerator.GetListOfPossibleLanguages();
            return response;
        }

        public async Task<string> SetLanguageAsync(ulong userDiscordId, string localeToSet)
        {
            try
            {
                if (!UserHelper.IsLocaleOrLanguage(localeToSet))
                {
                    string errorMessage = UserResponseGenerator.GetLanguageErrorMessage();
                    return errorMessage;
                }

                var id = userDiscordId;
                var locale = UserHelper.GetLocaleFromString(localeToSet);
                await UserService.SetUserLocale(id, locale);
                string response = UserResponseGenerator.GetLanguageSuccessMessage();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> EnableRemindersAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                await UserService.SetRemindersStateAsync(id, state: true);
                string response = UserResponseGenerator.GetEnableRemindersSuccessMessage();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> DisableRemindersAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                await UserService.SetRemindersStateAsync(id, state: false);
                string response = UserResponseGenerator.GetDisableRemindersSuccessMessage();
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
