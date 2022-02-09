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
        private UserResponseGenerator ResponseGenerator { get; }

        public UserCommandExecutor(
            ILogger logger,
            UserFacade userFacade,
            UserHelper userHelper,
            UserResponseGenerator responseGenerator) : base()
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
            UserHelper = userHelper 
                ?? throw new ArgumentNullException(nameof(userHelper));
            ResponseGenerator = responseGenerator 
                ?? throw new ArgumentNullException(nameof(responseGenerator));
        }

        public async Task<string> ListSettingsAsync(ulong userDiscordId)
        {
            try
            {
                var id = userDiscordId;
                var user = await UserFacade.ReadUserAndCreateIfNotExistsAsync(id);
                string response = ResponseGenerator.GetUserSettingsList(user);
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> ListLocales()
        {
            string response = ResponseGenerator.GetListOfPossibleLocales();
            return response;
        }

        public async Task<string> SetLocaleAsync(ulong userDiscordId, string localeToSet)
        {
            try
            {
                if (!UserHelper.IsLocale(localeToSet))
                {
                    string errorMessage = ResponseGenerator.GetLocaleErrorMessage();
                    return errorMessage;
                }

                var id = userDiscordId;
                var locale = UserHelper.GetLocaleFromString(localeToSet);
                await UserFacade.SetUserLocaleAsync(id, locale);
                string response = ResponseGenerator.GetLocaleSuccessMessage();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> ListLocations()
        {
            string response = ResponseGenerator.GetListOfPossibleLocations();
            return response;
        }

        public async Task<string> SetLocationAsync(ulong userDiscordId, string newLocation)
        {
            try
            {
                if (!UserHelper.IsLocation(newLocation))
                {
                    string errorMessage = ResponseGenerator.GetLocationErrorMessage();
                    return errorMessage;
                }

                var id = userDiscordId;
                await UserFacade.SetUserLocationAsync(id, newLocation);
                string response = ResponseGenerator.GetLocationSuccessMessage();
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }
    }
}
