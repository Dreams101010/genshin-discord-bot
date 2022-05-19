using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotUI.CommandExecutors;
using Autofac;

namespace GenshinDiscordBotUI.CommandModules
{
    public class UserCommandModule : ModuleBase<SocketCommandContext>
    {
		private ILifetimeScope Scope { get; }
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
		public async Task ListLocalesAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
            string response = userCommandExecutor.ListLocales();
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

		[Command("remindersOn")]
		public async Task EnableRemindersAsync()
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.EnableRemindersAsync(id);
			await ReplyAsync(response);
		}

		[Command("remindersOff")]
		public async Task DisableRemindersAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.DisableRemindersAsync(id);
			await ReplyAsync(response);
		}
	}
}
