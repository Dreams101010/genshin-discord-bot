using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotDomainLayer.CommandFacades;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using Autofac;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class UserCommandExecutor
    {
        private ILogger Logger { get; }
        private UserFacade UserFacade { get; }
        private UserHelper UserHelper { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private UserResponseGenerator UserResponseGenerator { get; }

        public UserCommandExecutor(
            ILogger logger,
            UserFacade userFacade,
            UserHelper userHelper,
            GeneralResponseGenerator generalResponseGenerator,
            UserResponseGenerator userResponseGenerator) : base()
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
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
                var user = await UserFacade.ReadUserAndCreateIfNotExistsAsync(id);
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

        public async Task<string> ListLocales()
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
                await UserFacade.SetUserLocaleAsync(id, locale);
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

        public async Task<string> ListLocations()
        {
            string response = UserResponseGenerator.GetListOfPossibleLocations();
            return response;
        }

        public async Task<string> SetLocationAsync(ulong userDiscordId, string newLocation)
        {
            try
            {
                if (!UserHelper.IsLocation(newLocation))
                {
                    string errorMessage = UserResponseGenerator.GetLocationErrorMessage();
                    return errorMessage;
                }

                var id = userDiscordId;
                await UserFacade.SetUserLocationAsync(id, newLocation);
                string response = UserResponseGenerator.GetLocationSuccessMessage();
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
