using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using Discord.WebSocket;
using Serilog;

namespace GenshinDiscordBotUI
{
    public class BotMessageSender : IBotMessageSender
    {
        public DiscordSocketClient Client { get; }
        private ILogger Logger { get; set; }

        public BotMessageSender(DiscordSocketClient client, ILogger logger)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendMessageAsync(MessageContext messageContext)
        {
            var guild = Client.GetGuild(messageContext.GuildId);
            if (guild == null)
            {
                Logger.Error("Guild was null");
                return;
            }
            var channel = guild.GetTextChannel(messageContext.ChannelId);
            if (channel == null)
            {
                Logger.Error("Channel was null");
                return;
            }
            var user = await Client.GetUserAsync(messageContext.DiscordUserId);
            if (user == null)
            {
                Logger.Error("User was null");
                return;
            }
            await channel.SendMessageAsync($"{user.Mention} {messageContext.Message}");
        }
    }
}
