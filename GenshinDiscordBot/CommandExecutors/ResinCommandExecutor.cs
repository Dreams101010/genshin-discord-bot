﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotUI.ResponseGenerators;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class ResinCommandExecutor
    {
        private ILogger Logger { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; }
        private ResinResponseGenerator ResinResponseGenerator { get; }
        private IUserService UserService { get; }
		private IResinService ResinService { get; }

        public ResinCommandExecutor(
			ILogger logger, 
			GeneralResponseGenerator generalResponseGenerator,
			ResinResponseGenerator resinResponseGenerator,
			IUserService userService,
			IResinService resinService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator 
				?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ResinResponseGenerator = resinResponseGenerator 
				?? throw new ArgumentNullException(nameof(resinResponseGenerator));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            ResinService = resinService ?? throw new ArgumentNullException(nameof(resinService));
        }

		public async Task<string> GetResinAsync(ulong userDiscordId)
		{
			try
			{
				var id = userDiscordId;
				var result = await ResinService.GetResinForUserAsync(id);
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

		public async Task<string> SetResinAsync(ulong userDiscordId, int newValue)
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
				var result = await ResinService.SetResinForUserAsync(id, newValue);
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
