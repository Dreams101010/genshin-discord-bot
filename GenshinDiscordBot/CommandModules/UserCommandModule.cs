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

namespace GenshinDiscordBotUI.CommandModules
{
    public class UserCommandModule : ModuleBase<SocketCommandContext>
    {
        public ILifetimeScope Scope { get; }
        public UserHelper UserHelper { get; }
        public ILogger Logger { get; }
        public UserResponseGenerator ResponseGenerator { get; }

        public UserCommandModule(ILifetimeScope scope, 
			UserHelper userHelper, 
			ILogger logger,
			UserResponseGenerator responseGenerator) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResponseGenerator = responseGenerator 
				?? throw new ArgumentNullException(nameof(responseGenerator));
        }

		[Command("settings_list")]
		public async Task ListSettingsAsync()
		{
			try
            {
				using var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				var id = Context.Message.Author.Id;
				var user = await userFacade.ReadUserAndCreateIfNotExistsAsync(id);
				string response = ResponseGenerator.GetUserSettingsList(user);
				await ReplyAsync(response);
			}
			catch
            {
				string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
				await ReplyAsync(errorMessage);
			}
		}

		[Command("set_locale")]
		public async Task ListLocales()
		{
			string response = ResponseGenerator.GetListOfPossibleLocales();
			await ReplyAsync(response);
		}

		[Command("set_locale")]
		public async Task SetLocaleAsync(string localeToSet)
        {
			try
			{
				using var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				if (!UserHelper.IsLocale(localeToSet))
                {
					string errorMessage = ResponseGenerator.GetLocaleErrorMessage();
					await ReplyAsync();
					return;
                }

				var id = Context.Message.Author.Id;
				var locale = UserHelper.GetLocaleFromString(localeToSet);
				await userFacade.SetUserLocaleAsync(id, locale);
				string response = ResponseGenerator.GetLocaleSuccessMessage();
				await ReplyAsync(response);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
				await ReplyAsync(errorMessage);
			}
		}

		[Command("set_location")]
		public async Task ListLocations()
        {
			string response = ResponseGenerator.GetListOfPossibleLocations();
			await ReplyAsync(response);
		}

		[Command("set_location")]
		public async Task SetLocationAsync(string newLocation)
        {
			try
			{
				using var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				if (!UserHelper.IsLocation(newLocation))
				{
					string errorMessage = ResponseGenerator.GetLocationErrorMessage();
					await ReplyAsync(errorMessage);
					return;
				}

				var id = Context.Message.Author.Id;
				await userFacade.SetUserLocationAsync(id, newLocation);
				string response = ResponseGenerator.GetLocationSuccessMessage();
				await ReplyAsync(response);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = ResponseGenerator.GetGeneralErrorMessage();
				await ReplyAsync(errorMessage);
			}
		}
	}
}
