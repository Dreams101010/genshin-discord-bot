using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GenshinDiscordBotUI
{
    public class Bot
    {
        private DiscordSocketClient Client { get; }
        private CommandHandler CommandHandler { get; }
        private ILogger Logger { get; }
        private IConfigurationRoot Configuration { get;  }
        private string Token { get; }
        public Bot(
            DiscordSocketClient client, 
            CommandHandler commandHandler, 
            IConfigurationRoot root,
            ILogger logger)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            CommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            Configuration = root ?? throw new ArgumentNullException(nameof(root));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            client.Log += Log;
            Token = Configuration.GetSection("Discord")["Token"];
        }
        public async Task StartBot()
        {
            await CommandHandler.InstallCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
        }

        private Task Log(LogMessage msg)
        {
            Logger.Information(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
