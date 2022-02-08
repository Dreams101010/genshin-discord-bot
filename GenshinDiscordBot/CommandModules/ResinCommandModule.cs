using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using GenshinDiscordBotDomainLayer.CommandFacades;
using GenshinDiscordBotUI.Helpers;
using Autofac;
using Serilog;

namespace GenshinDiscordBotUI.CommandModules
{
    public class ResinCommandModule : ModuleBase<SocketCommandContext>
    {
        public ILifetimeScope Scope { get; }
        public UserHelper UserHelper { get; }
        public ILogger Logger { get; }

        public ResinCommandModule(ILifetimeScope scope, 
            UserHelper userHelper, 
            ILogger logger) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            UserHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
