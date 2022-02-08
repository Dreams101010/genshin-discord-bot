using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using GenshinDiscordBotDomainLayer.Facades;
using GenshinDiscordBotUI.Helpers;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using GenshinDiscordBotDomainLayer.DomainModels;
using Serilog;

namespace GenshinDiscordBotUI.CommandModules
{
    public class GeneralCommandModule : ModuleBase<SocketCommandContext>
    {
        public ILifetimeScope Scope { get; }
        public UserHelper UserHelper { get; }
        public ILogger Logger { get; }

        public GeneralCommandModule(ILifetimeScope scope, UserHelper userHelper, ILogger logger) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

		[Command("settings_list")]
		public async Task ListSettingsAsync()
		{
			Console.WriteLine("In ListSettings");
			try
            {
				using var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				var id = Context.Message.Author.Id;
				var user = await userFacade.ReadUserAndCreateIfNotExistsAsync(id);
				await ReplyAsync($"Locale: {user.Locale}, Location: {user.Location}");
			}
			catch
            {
				await ReplyAsync(@$"Something went wrong. 
								Please contact the developer. 
								The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
		}

		[Command("set_locale")]
		public async Task ListLocales()
		{
			await ReplyAsync(
				"Possible locales are: \n" +
				"ruRU \n" +
				"enGB \n");
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
					await ReplyAsync("Incorrect locale setting. Correct settings are ruRU and enGB");
					return;
                }

				var id = Context.Message.Author.Id;
				var locale = UserHelper.GetLocaleFromString(localeToSet);
				await userFacade.SetUserLocaleAsync(id, locale);
				await ReplyAsync("Locale has been set.");
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				await ReplyAsync($"Something went wrong. " +
					$"Please contact the developer. " +
					$"The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
		}

		[Command("set_location")]
		public async Task ListLocations()
        {
			await ReplyAsync(
				"Possible locations are: \n" +
				"\"Not specified\" \n" +
				"\"Moscow, Russia\" \n" +
				"\"Saint Petersburg, Russia\" \n" +
				"\"London, Great Britain\"");
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
					await ReplyAsync(
						"Incorrect location setting. Correct settings are: \n" +
						"Not specified \n" +
						"Moscow, Russia \n" +
						"Saint Petersburg, Russia \n" +
						"London, Great Britain");
					return;
				}

				var id = Context.Message.Author.Id;
				await userFacade.SetUserLocationAsync(id, newLocation);
				await ReplyAsync("Location has been set.");
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				await ReplyAsync($"Something went wrong. " +
					$"Please contact the developer. " +
					$"The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
		}

		[Command("getResin")]
		public async Task GetResin()
        {
			try
            {
				using var scope = Scope.BeginLifetimeScope();
				var resinFacade = scope.Resolve<ResinFacade>();
				var userFacade = scope.Resolve<UserFacade>();
				var id = Context.Message.Author.Id;
				await userFacade.CreateUserIfNotExistsAsync(id);
				var result = await resinFacade.GetResinForUser(id);
				if (result.HasValue)
                {
					var resinInfo = result.Value;
					await ReplyAsync($"Your resin count is {resinInfo.CurrentCount}. " +
						$"Time to full resin is {resinInfo.TimeToFullResin} " +
						$"({resinInfo.CompletionTime} UTC)");
				}
				else
                {
					await ReplyAsync($"Could not get resin count for you. Perhaps resin has not been set?");
				}
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				await ReplyAsync($"Something went wrong. " +
					$"Please contact the developer. " +
					$"The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
        }

		[Command("setResin")]
		public async Task SetResin(int newValue)
        {
			if (newValue > 160 || newValue < 0)
            {
				await ReplyAsync("Invalid resin value");
				return;
            }
			try
			{
				using var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				var resinFacade = scope.Resolve<ResinFacade>();
				var id = Context.Message.Author.Id;
				await userFacade.CreateUserIfNotExistsAsync(id);
				var result = await resinFacade.SetResinForUser(id, newValue);
				await ReplyAsync($"Resin has been set.");
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				await ReplyAsync($"Something went wrong. " +
					$"Please contact the developer. " +
					$"The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
		}
	}
}
