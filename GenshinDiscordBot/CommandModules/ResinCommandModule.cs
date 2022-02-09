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
		private UserHelper UserHelper { get; }
		private ILogger Logger { get; }
		private ResinResponseGenerator ResinResponseGenerator { get; }

        public ResinCommandModule(ILifetimeScope scope, 
            UserHelper userHelper, 
            ILogger logger, 
			ResinResponseGenerator resinResponseGenerator) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResinResponseGenerator = resinResponseGenerator 
				?? throw new ArgumentNullException(nameof(resinResponseGenerator));
        }

		[Command("getResin")]
		public async Task GetResin()
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await resinCommandExecutor.GetResin(id);
			await ReplyAsync(response);
		}

		[Command("setResin")]
		public async Task SetResin(int newValue)
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
			var id = Context.Message.Author.Id;
			string response = await resinCommandExecutor.SetResin(id, newValue);
			await ReplyAsync(response);
		}
	}
}
