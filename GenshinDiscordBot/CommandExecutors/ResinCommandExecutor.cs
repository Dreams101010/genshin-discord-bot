using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.CommandFacades;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class ResinCommandExecutor
    {
        private ILogger Logger { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private ResinResponseGenerator ResinResponseGenerator { get; }
        private UserFacade UserFacade { get; }
		private ResinFacade ResinFacade { get; }

        public ResinCommandExecutor(
			ILogger logger, 
			GeneralResponseGenerator generalResponseGenerator,
			ResinResponseGenerator resinResponseGenerator,
			UserFacade userFacade,
			ResinFacade resinFacade)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator 
				?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ResinResponseGenerator = resinResponseGenerator 
				?? throw new ArgumentNullException(nameof(resinResponseGenerator));
            UserFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
            ResinFacade = resinFacade ?? throw new ArgumentNullException(nameof(resinFacade));
        }

		public async Task<string> GetResin(ulong userDiscordId)
		{
			try
			{
				var id = userDiscordId;
				var result = await ResinFacade.GetResinForUser(id);
				if (result.HasValue)
				{
					var resinInfo = result.Value;
					string response = ResinResponseGenerator.GetGetResinSuccessResponse(resinInfo);
					return response;
				}
				else
				{
					string errorMessage = ResinResponseGenerator.GetGetResinErrorMessage();
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

		public async Task<string> SetResin(ulong userDiscordId, int newValue)
		{
			string validationErrorMessage = ResinResponseGenerator
				.GetSetResinValidationErrorMessage(newValue);
			if (validationErrorMessage != null)
            {
				return validationErrorMessage;
            }
			try
			{
				var id = userDiscordId;
				var result = await ResinFacade.SetResinForUser(id, newValue);
				string response = ResinResponseGenerator.GetSetResinSuccessMessage();
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
