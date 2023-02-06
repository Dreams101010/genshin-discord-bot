using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotDomainLayer.Contexts;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class UserCommandExecutor : CommandExecutorBase
    {
        private ILogger Logger { get; }
        private IUserService UserService { get; }
        private UserHelper UserHelper { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private UserResponseGenerator UserResponseGenerator { get; }
        private RequestContext Context { get;  } 

        public UserCommandExecutor(
            ILogger logger,
            IUserService userService,
            UserHelper userHelper,
            GeneralResponseGenerator generalResponseGenerator,
            UserResponseGenerator userResponseGenerator,
            RequestContext requestContext) : base(userService, logger, requestContext)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            UserHelper = userHelper 
                ?? throw new ArgumentNullException(nameof(userHelper));
            GeneralResponseGenerator = generalResponseGenerator ?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            UserResponseGenerator = userResponseGenerator 
                ?? throw new ArgumentNullException(nameof(userResponseGenerator));
            Context = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public async Task<string> GetHelpMessageAsync()
        {
            try
            {
                var userLocale = Context.UserContext.User.Locale;
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

        public async Task<string> ListSettingsAsync()
        {
            try
            {
                var user = Context.UserContext.User;
                var userName = Context.DiscordContext.UserName;
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

        public async Task<string> ListLanguagesAsync()
        {
            var userName = Context.DiscordContext.UserName;
            var userLocale = Context.UserContext.User.Locale;
            string response = UserResponseGenerator.GetListOfPossibleLanguages(userLocale, userName);
            return response;
        }

        public async Task<string> SetLanguageAsync(string localeToSet)
        {
            try
            {
                var userId = Context.DiscordContext.UserId;
                var userName = Context.DiscordContext.UserName;
                var originalUserLocale = Context.UserContext.User.Locale;
                if (!UserHelper.IsLocaleOrLanguage(localeToSet))
                {
                    string errorMessage = UserResponseGenerator.GetLanguageErrorMessage(
                        originalUserLocale, userName);
                    return errorMessage;
                }
                var newLocale = UserHelper.GetLocaleFromString(localeToSet);
                await UserService.SetUserLocale(userId, newLocale);
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

        public async Task<string> EnableRemindersAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
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

        public async Task<string> DisableRemindersAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
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
