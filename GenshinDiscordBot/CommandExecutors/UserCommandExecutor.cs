using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Services;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class UserCommandExecutor
    {
        private ILogger Logger { get; }
        private UserService UserService { get; }
        private UserHelper UserHelper { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private UserResponseGenerator UserResponseGenerator { get; }

        public UserCommandExecutor(
            ILogger logger,
            UserService userService,
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

        public string ListSettingsAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                var user = UserService.ReadUserAndCreateIfNotExists(id);
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

        public string SetLocaleAsync(ulong userDiscordId, string localeToSet)
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
                UserService.SetUserLocale(id, locale);
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
