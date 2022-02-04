using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using GenshinDiscordBotDomainLayer.Facades;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace GenshinDiscordBotUI.CommandModules
{
    public class GeneralCommandModule : ModuleBase<SocketCommandContext>
    {
        public ILifetimeScope Scope { get; }

        public GeneralCommandModule(ILifetimeScope scope) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

		[Command("sayhi")]
		[Summary
		("Says hi to current user.")]
		[Alias("hi", "hello")]
		public async Task SayHiAsync()
		{
			var users = Context.Guild.Users;
			SocketGuildUser userToSend = null;
			foreach (var user in users)
			{
				if (user.Id == Context.Message.Author.Id)
				{
					userToSend = user;
				}
			}
			if (userToSend != null)
			{
				await ReplyAsync($"Hi, {userToSend.Nickname}");
			}
		}

		[Command("settings_list")]
		public async Task ListSettingsAsync()
		{
			try
            {
				var scope = Scope.BeginLifetimeScope();
				var userFacade = scope.Resolve<UserFacade>();
				var id = Context.Message.Author.Id;
				var user = await userFacade.ReadUserAndCreateIfNotExistsAsync(id);
				await ReplyAsync($"Locale: {user.Locale}, Location: {user.Location}");
				await scope.DisposeAsync();
			}
			catch
            {
				await ReplyAsync(@$"Something went wrong. 
								Please contact the developer. 
								The time of the event: {DateTime.Now.ToUniversalTime()}");
			}
		}
	}
}
