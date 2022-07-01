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

		[Command("help")]
		public async Task PrintHelpMessage()
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.GetHelpMessageAsync(id, userName);
			await ReplyAsync(response);
		}

		[Command("printSettings")]
		public async Task ListSettingsAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.ListSettingsAsync(id, userName);
			await ReplyAsync(response);
		}

		[Command("setLanguage")]
		public async Task ListLocalesAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.ListLanguagesAsync(id, userName);
            await ReplyAsync(response);
		}

		[Command("setLanguage")]
		public async Task SetLocaleAsync(string localeToSet)
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.SetLanguageAsync(id, localeToSet, userName);
			await ReplyAsync(response);
		}

		[Command("remindersOn")]
		public async Task EnableRemindersAsync()
        {
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.EnableRemindersAsync(id, userName);
			await ReplyAsync(response);
		}

		[Command("remindersOff")]
		public async Task DisableRemindersAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var userCommandExecutor = scope.Resolve<UserCommandExecutor>();
			var userName = Context.Message.Author.Username;
			var id = Context.Message.Author.Id;
			string response = await userCommandExecutor.DisableRemindersAsync(id, userName);
			await ReplyAsync(response);
		}

		[Command("ehe")]
		public async Task HeheAsync()
        {
			await ReplyAsync("EHE TE NANDAYO?");
        }
	}
}
