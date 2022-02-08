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
using GenshinDiscordBotUI.ResponseGenerators;

namespace GenshinDiscordBotUI.CommandModules
{
    public class ResinCommandModule : ModuleBase<SocketCommandContext>
    {
        public ILifetimeScope Scope { get; }
        public UserHelper UserHelper { get; }
        public ILogger Logger { get; }
        public ResinResponseGenerator ResinResponseGenerator { get; }

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
			try
			{
				using var scope = Scope.BeginLifetimeScope();
				var resinFacade = scope.Resolve<ResinFacade>();
				var id = Context.Message.Author.Id;
				var result = await resinFacade.GetResinForUser(id);
				if (result.HasValue)
				{
					var resinInfo = result.Value;
					string response = ResinResponseGenerator.GetGetResinSuccessResponse(resinInfo);
					await ReplyAsync(response);
				}
				else
				{
					string errorMessage = ResinResponseGenerator.GetGetResinErrorMessage();
					await ReplyAsync(errorMessage);
				}
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = ResinResponseGenerator.GetGeneralErrorMessage();
				await ReplyAsync(errorMessage);
			}
		}

		[Command("setResin")]
		public async Task SetResin(int newValue)
		{
			if (newValue > 160 || newValue < 0)
			{
				string errorMessage = ResinResponseGenerator.GetSetResinErrorMessage();
				await ReplyAsync(errorMessage);
				return;
			}
			try
			{
				using var scope = Scope.BeginLifetimeScope();
				var resinFacade = scope.Resolve<ResinFacade>();
				var id = Context.Message.Author.Id;
				var result = await resinFacade.SetResinForUser(id, newValue);
				string response = ResinResponseGenerator.GetSetResinSuccessMessage();
				await ReplyAsync(response);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = ResinResponseGenerator.GetGeneralErrorMessage();
				await ReplyAsync(errorMessage);
			}
		}
	}
}
