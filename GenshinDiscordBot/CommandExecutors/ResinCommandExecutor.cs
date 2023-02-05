using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Contexts;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class ResinCommandExecutor : CommandExecutorBase
    {
        private ILogger Logger { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private ResinResponseGenerator ResinResponseGenerator { get; }
		private RequestContext Context { get; }
        private IUserService UserService { get; }
		private IResinService ResinService { get; }

		public ResinCommandExecutor(
			ILogger logger, 
			GeneralResponseGenerator generalResponseGenerator,
			ResinResponseGenerator resinResponseGenerator,
			IUserService userService,
			IResinService resinService,
            RequestContext requestContext) : base(userService, requestContext)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator 
				?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ResinResponseGenerator = resinResponseGenerator 
				?? throw new ArgumentNullException(nameof(resinResponseGenerator));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            ResinService = resinService ?? throw new ArgumentNullException(nameof(resinService));
			Context = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
		}

		public async Task<string> GetResinAsync()
		{
			try
			{
				var id = Context.DiscordContext.UserId;
				var userName = Context.DiscordContext.UserName;
                var userLocale = (await UserService.ReadUserAndCreateIfNotExistsAsync(id)).Locale;
				var resinInfo = await ResinService.GetResinForUserAsync(id);
				if (!resinInfo.IsEmpty)
				{
					string response = ResinResponseGenerator.GetGetResinSuccessResponse(
						userLocale, resinInfo, userName);
					return response;
				}
				else
				{
					string errorMessage = ResinResponseGenerator.GetGetResinErrorMessage(
						userLocale, userName);
					return errorMessage;
				}
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
				return errorMessage;
			}
		}

		public async Task<string> SetResinAsync(int newValue)
		{
            var id = Context.DiscordContext.UserId;
            var userName = Context.DiscordContext.UserName;
			var userLocale = Context.UserContext.User.Locale;
			string validationErrorMessage = ResinResponseGenerator
				.GetSetResinValidationErrorMessage(userLocale, newValue, userName);
			if (validationErrorMessage != string.Empty)
            {
				return validationErrorMessage;
            }
			try
			{
				var result = await ResinService.SetResinForUserAsync(id, newValue);
				string response = ResinResponseGenerator.GetSetResinSuccessMessage(userLocale, userName);
				return response;
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
				return errorMessage;
			}
		}
	}
}
