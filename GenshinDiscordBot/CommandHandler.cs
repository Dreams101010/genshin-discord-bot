using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Autofac;
using Discord;
using System.Configuration;

namespace GenshinDiscordBotUI
{
    public class CommandHandler
    {
        private DiscordSocketClient Client { get; }
        private CommandService Commands { get; }
        // To provide to AddModulesAsync (needed to resolve dependencies
        // during CommandModule class construction)
        public IServiceProvider ServiceProvider { get; }
        public ILifetimeScope Scope { get; }

        public CommandHandler(DiscordSocketClient client,
            CommandService commands,
            IServiceProvider provider,
            ILifetimeScope scope)
        {
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
            Client = client ?? throw new ArgumentNullException(nameof(client));
            ServiceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await Commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
            services: ServiceProvider);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Create scope for handling this command
            using var localScope = Scope.BeginLifetimeScope();
            // Resolve IServiceProvider to provide to ExecuteAsync
            var scopedServices = localScope.Resolve<IServiceProvider>();

            // Don't process the command if it was a system message
            if (messageParam is not SocketUserMessage message) return;

            // Don't process the command if it is not a DM command or a text channel command
            var channelType = messageParam.Channel.GetChannelType();
            if (channelType == null)
            {
                return;
            }
            switch (channelType)
            {
                case ChannelType.DM:
                case ChannelType.Text:
                    break;
                default:
                    return;
            }

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(Client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(Client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await Commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: scopedServices);
        }
    }
}
