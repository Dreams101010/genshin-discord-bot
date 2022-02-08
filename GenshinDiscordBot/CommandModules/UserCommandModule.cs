using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotDomainLayer.Facades;
using GenshinDiscordBotUI.Helpers;
using Autofac;
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
	}
}
