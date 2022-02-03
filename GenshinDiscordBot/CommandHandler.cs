using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;

namespace GenshinDiscordBotUI
{
    public class CommandHandler
    {
        private DiscordSocketClient Client { get; }
        private CommandService Commands { get; }

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await Commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (messageParam is not SocketUserMessage message) return;

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
                services: null);
        }
    }
}
