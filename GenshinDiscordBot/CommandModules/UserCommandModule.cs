using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotDomainLayer.CommandFacades;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.CommandExecutors;
using Autofac;
using Serilog;

namespace GenshinDiscordBotUI.CommandModules
{
    public class UserCommandModule : ModuleBase<SocketCommandContext>
    {
		private ILifetimeScope Scope { get; }
		private UserCommandExecutor UserCommandExecutor { get; }

        public UserCommandModule(
			ILifetimeScope scope) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

		[Command("settings_list")]
		public async Task ListSettingsAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.ListSettingsAsync(id);
			await ReplyAsync(response);
		}

		[Command("set_locale")]
		public async Task ListLocales()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
            string response = await userCommandExecutor.ListLocales();
            await ReplyAsync(response);
		}

		[Command("set_locale")]
		public async Task SetLocaleAsync(string localeToSet)
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.SetLocaleAsync(id, localeToSet);
			await ReplyAsync(response);
		}

		[Command("set_location")]
		public async Task ListLocations()
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			string response = await userCommandExecutor.ListLocations();
			await ReplyAsync(response);
		}

		[Command("set_location")]
		public async Task SetLocationAsync(string newLocation)
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.SetLocationAsync(id, newLocation);
			await ReplyAsync(response);
		}
	}
}
