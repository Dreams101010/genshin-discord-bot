using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace GenshinDiscordBotUI.CommandModules
{
    public class GeneralCommandModule : ModuleBase<SocketCommandContext>
    {
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
	}
}
