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

        public string ListLocales()
        {
            string response = UserResponseGenerator.GetListOfPossibleLocales();
            return response;
        }

        public async Task<string> SetLocaleAsync(ulong userDiscordId, string localeToSet)
        {
            try
            {
                if (!UserHelper.IsLocale(localeToSet))
                {
                    string errorMessage = UserResponseGenerator.GetLocaleErrorMessage();
                    return errorMessage;
                }

                var id = userDiscordId;
                var locale = UserHelper.GetLocaleFromString(localeToSet);
                await UserService.SetUserLocale(id, locale);
                string response = UserResponseGenerator.GetLocaleSuccessMessage();
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
