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
using System.Windows.Forms;
using System.ComponentModel;
using GenshinDiscordBotUI.Models.SlashCommand;
using GenshinDiscordBotUI.SlashCommands;

namespace GenshinDiscordBotUI
{
    public class Bot
    {
        private DiscordSocketClient Client { get; }
        private CommandHandler CommandHandler { get; }
        public IReminderDispatcherService ReminderDispatcherService { get; }
        public INotificationService NotificationService { get; }
        private ILogger Logger { get; }
        private IConfigurationRoot Configuration { get; }
        private SlashCommandDispatcher SlashCommandDispatcher { get; }
        private string Token { get; }
        private bool IsInitialized { get; set; } = false;
        public Bot(
            DiscordSocketClient client, 
            CommandHandler commandHandler,
            IReminderDispatcherService reminderDispatcherService,
            INotificationService notificationService,
            IConfigurationRoot root,
            SlashCommandDispatcher slashCommandDispatcher,
            ILogger logger)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            CommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            ReminderDispatcherService = reminderDispatcherService ?? throw new ArgumentNullException(nameof(reminderDispatcherService));
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            Configuration = root ?? throw new ArgumentNullException(nameof(root));
            SlashCommandDispatcher = slashCommandDispatcher ?? throw new ArgumentNullException(nameof(slashCommandDispatcher));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            client.Ready += On_ClientReady;
            client.Log += Log;
            client.ButtonExecuted += ButtonHandler;
            client.SlashCommandExecuted += On_SlashCommandExecution;
            Token = Configuration.GetSection("Discord")["Token"];
        }
        public async Task StartBot(CancellationToken token)
        {
            if (!IsInitialized)
            {
                await CommandHandler.InstallCommandsAsync();
                IsInitialized = true;
            }
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            _ = ReminderDispatcherService.DispatcherAsync(token);
            NotificationService.Start();
            await CancellationLoop(token);
            await Client.LogoutAsync();
        }

        private async Task CancellationLoop(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                await Task.Delay(5000, CancellationToken.None);
            }
        }

        private Task Log(LogMessage msg)
        {
            Logger.Information(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task On_ClientReady()
        {
            if (Configuration.GetRequiredSection("Configuration")["SetGlobalCommands"] == "true")
            {
                System.Diagnostics.Debug.WriteLine("init");
                await SlashCommandDispatcher.RegisterSlashCommandsAsync(Client);
            }
        }

        private async Task On_SlashCommandExecution(SocketSlashCommand command)
        {
            var response = await SlashCommandDispatcher.DispatchAsync(command);
            await command.RespondAsync(response, ephemeral: true);
        }

        public async Task ButtonHandler(SocketMessageComponent component)
        {
            // only works with guilds
            var user = component.User as SocketGuildUser;
            if (user == null)
            {
                Logger.Error("Couldn't get guild user while handling a component button press");
                await component.RespondAsync("Sorry, something went wrong. Please contact my creator", ephemeral: true);
                return;
            }
            var channel = component.Channel as SocketGuildChannel;
            if (channel == null)
            {
                Logger.Error("Couldn't get guild channel while handling a component button press");
                await component.RespondAsync("Sorry, something went wrong. Please contact my creator", ephemeral: true);
                return;
            }
            var guild = channel.Guild;
            var genshinPromocodesRole = 
                guild.Roles.Where((x) => x.Name == "Genshin Promocodes Role").FirstOrDefault();
            if (genshinPromocodesRole == null)
            {
                Logger.Error("Couldn't get Genshin Promocodes role while handling a component button press");
                await component.RespondAsync("Sorry, something went wrong. Please contact my creator", ephemeral: true);
                return;
            }
            var honkaiPromocodesRole = 
                guild.Roles.Where((x) => x.Name == "Honkai Impact 3rd Promocodes Role").FirstOrDefault();
            if (honkaiPromocodesRole == null)
            {
                Logger.Error("Couldn't get Honkai Impact 3rd Promocodes role while handling a component button press");
                await component.RespondAsync("Sorry, something went wrong. Please contact my creator", ephemeral: true);
                return;
            }
            switch (component.Data.CustomId)
            {
                // Since we set our buttons custom id as 'custom-id', we can check for it like this:
                case "genshin-promocode-role-give":
                    await user.AddRoleAsync(genshinPromocodesRole);
                    await component.RespondAsync("I have given you the Genshin Promocodes role.", ephemeral: true);
                    break;
                case "genshin-promocode-role-remove":
                    await user.RemoveRoleAsync(genshinPromocodesRole);
                    await component.RespondAsync("I have taken the Genshin Promocodes role from you.", ephemeral: true);
                    break;
                case "honkai-promocode-role-give":
                    await user.AddRoleAsync(honkaiPromocodesRole);
                    await component.RespondAsync("I have given you the Honkai Impact 3rd Promocodes role.", ephemeral: true);
                    break;
                case "honkai-promocode-role-remove":
                    await user.RemoveRoleAsync(honkaiPromocodesRole);
                    await component.RespondAsync("I have taken the Honkai Impact 3rd Promocodes role from you.", ephemeral: true);
                    break;
            }
        }

        public DiscordSocketClient GetClient() => Client;
    }
}
