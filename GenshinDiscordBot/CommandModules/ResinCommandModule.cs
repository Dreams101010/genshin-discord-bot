﻿using System;
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
		public async Task GetResinAsync()
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
			resinCommandExecutor.PopulateContextAsync(Context);
			string response = await resinCommandExecutor.GetResinAsync();
			await ReplyAsync(response);
		}

		[Command("setResin")]
		public async Task SetResinAsync(int newValue)
		{
			using var scope = Scope.BeginLifetimeScope();
			var resinCommandExecutor = scope.Resolve<ResinCommandExecutor>();
            resinCommandExecutor.PopulateContextAsync(Context);
			string response = await resinCommandExecutor.SetResinAsync(newValue);
			await ReplyAsync(response);
		}
	}
}
