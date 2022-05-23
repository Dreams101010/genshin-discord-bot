using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Serilog;
using GenshinDiscordBotDomainLayer.Interfaces.Services;

namespace GenshinDiscordBotUI
{
    public class Bot
    {
        private DiscordSocketClient Client { get; }
        private CommandHandler CommandHandler { get; }
        public IReminderDispatcherService ReminderDispatcherService { get; }
        private ILogger Logger { get; }
        private IConfigurationRoot Configuration { get;  }
        private string Token { get; }
        public Bot(
            DiscordSocketClient client, 
            CommandHandler commandHandler,
            IReminderDispatcherService reminderDispatcherService,
            IConfigurationRoot root,
            ILogger logger)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            CommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            ReminderDispatcherService = reminderDispatcherService ?? throw new ArgumentNullException(nameof(reminderDispatcherService));
            Configuration = root ?? throw new ArgumentNullException(nameof(root));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            client.Log += Log;
            Token = Configuration.GetSection("Discord")["Token"];
        }
        public async Task StartBot(CancellationToken token)
        {
            await CommandHandler.InstallCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await ReminderDispatcherService.DispatcherAsync(token);
        }

        private Task Log(LogMessage msg)
        {
            Logger.Information(msg.ToString());
            return Task.CompletedTask;
        }

        public DiscordSocketClient GetClient() => Client;
    }
}
