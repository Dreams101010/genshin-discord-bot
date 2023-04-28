using Discord;
using Discord.WebSocket;
using GenshinDiscordBotUI.CommandExecutors;
using GenshinDiscordBotUI.Models.SlashCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GenshinDiscordBotUI.SlashCommands
{
    public class UserSlashCommandHandler
    {
        UserCommandExecutor Executor { get; set; }

        public UserSlashCommandHandler(UserCommandExecutor executor)
        {
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public async Task<string> HandleAsync(SocketSlashCommand command)
        {
            Executor.PopulateContextAsync(command);
            string response = command.CommandName switch
            {
                "help" => await Executor.GetHelpMessageAsync(),
                "help_for_command" => await HandleHelpForCommandCommandAsync(command),
                "print_settings" => await Executor.ListSettingsAsync(),
                "list_languages" => await Executor.ListLanguagesAsync(),
                "set_language" => await HandleSetLanguageCommandAsync(command),
                "reminders_on" => await Executor.EnableRemindersAsync(),
                "reminders_off" => await Executor.DisableRemindersAsync(),
                _ => throw new NotImplementedException("Unknown slash command in category: user"),
            };
            return response;
        }

        private async Task<string> HandleHelpForCommandCommandAsync(
            SocketSlashCommand command)
        {
            if (command.CommandName != "help_for_command")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandName = command.Data.Options.Where(x => x.Name == "command_name").First().Value as string;
            if (paramCommandName == null)
            {
                throw new Exception("Required parameter was null: command_name");
            }
            return await Executor.GetHelpMessageForCommandAsync(paramCommandName);
        }

        private async Task<string> HandleSetLanguageCommandAsync(
            SocketSlashCommand command)
        {
            if (command.CommandName != "set_language")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandName = command.Data.Options.Where(x => x.Name == "language").First().Value as string;
            if (paramCommandName == null)
            {
                throw new Exception("Required parameter was null: language");
            }
            return await Executor.SetLanguageAsync(paramCommandName);
        }
    }
}
