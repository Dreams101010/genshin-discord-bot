using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotUI.Helpers;
using Autofac;
using Serilog;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.CommandExecutors;

namespace GenshinDiscordBotUI.CommandModules
{
    public class ResinCommandModule : ModuleBase<SocketCommandContext>
    {
		private ILifetimeScope Scope { get; }

        public ResinCommandModule(ILifetimeScope scope) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

		[Command("getResin")]
		public async Task GetResin()
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = resinCommandExecutor.GetResin(id);
			await ReplyAsync(response);
		}

		[Command("setResin")]
		public async Task SetResin(int newValue)
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = resinCommandExecutor.SetResin(id, newValue);
			await ReplyAsync(response);
		}
	}
}
