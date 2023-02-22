using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI
{
    internal class DiscordNotifier : INotifier
    {
        IBotMessageSender BotMessageSender { get; }

        public DiscordNotifier(IBotMessageSender botMessageSender)
        {
            BotMessageSender = botMessageSender ?? throw new ArgumentNullException(nameof(botMessageSender));
        }
        public async Task<bool> Notify(string message, NotificationEndpoint endpoint)
        {
            MessageContext context = new()
            {
                Message = message,
                DiscordUserId = endpoint.UserDiscordId,
                GuildId = endpoint.GuildId,
                ChannelId = endpoint.ChannelId
            };
            return await BotMessageSender.SendMessageAsync(context);
        }
    }
}
