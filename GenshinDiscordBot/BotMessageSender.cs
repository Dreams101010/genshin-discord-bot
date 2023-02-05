using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using Discord.WebSocket;
using Serilog;
using Discord;

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

        public async Task<bool> SendMessageAsync(MessageContext messageContext)
        {
            int retryCount = 5;
            int currentRetry = 0;
            while (currentRetry < retryCount)
            {
                try
                {
                    if (currentRetry > 1)
                    {
                        Logger.Information(string.Format("Sending message. Retry {0}", currentRetry));
                        await Task.Delay(currentRetry * 50);
                    }
                    if (IsDmMessage(messageContext))
                    {
                        return await SendDmMessage(messageContext);
                    }
                    else
                    {
                        return await SendChannelMessageAsync(messageContext);
                    }
                }
                catch (Exception ex)
                {
                    if (currentRetry == retryCount)
                    {
                        Logger.Error(
                            string.Format("Message has not been sent after {0} retries", currentRetry));
                        Logger.Error(ex, "Exception has occured while trying to send message");
                        break;
                    }
                    currentRetry++;
                }
            }
            return false;
        }

        private async Task<bool> SendChannelMessageAsync(MessageContext messageContext)
        {
            var guild = Client.GetGuild(messageContext.GuildId);
            if (guild == null)
            {
                Logger.Error("Guild was null while trying to send message");
                return false;
            }
            var channel = guild.GetTextChannel(messageContext.ChannelId);
            if (channel == null)
            {
                Logger.Error("Channel was null while trying to send message");
                return false;
            }
            var user = await Client.GetUserAsync(messageContext.DiscordUserId);
            if (user == null)
            {
                Logger.Error("User was null while trying to send message");
                return false;
            }
            await channel.SendMessageAsync($"{user.Mention} {messageContext.Message}");
            return true;
        }

        private async Task<bool> SendDmMessage(MessageContext messageContext)
        {
            var user = await Client.GetUserAsync(messageContext.DiscordUserId);
            if (user == null)
            {
                Logger.Error("User was null while trying to send message");
                return false;
            }
            await user.SendMessageAsync($"{user.Mention} {messageContext.Message}");
            return true;
        }

        private bool IsDmMessage(MessageContext context)
        {
            return context.GuildId == 0 && context.ChannelId == 0;
        }
    }
}
