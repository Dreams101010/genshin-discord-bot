using Discord.WebSocket;
using GenshinDiscordBotUI.CommandExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.SlashCommands
{
    public class ResinSlashCommandHandler
    {
        ResinCommandExecutor Executor { get; set; }

        public ResinSlashCommandHandler(ResinCommandExecutor executor)
        {
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public async Task<string> HandleAsync(SocketSlashCommand command)
        {
            Executor.PopulateContextAsync(command);
            string response = command.CommandName switch
            {
                "get_resin" => await Executor.GetResinAsync(),
                "set_resin" => await HandleSetResinCommandAsync(command),
                _ => throw new NotImplementedException("Unknown slash command in category: resin"),
            };
            return response;
        }

        private async Task<string> HandleSetResinCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "set_resin")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandName = command.Data.Options.Where(x => x.Name == "resin_amount").First().Value;
            if (paramCommandName == null)
            {
                throw new Exception("Required parameter was null: resin_amount");
            }
            if (!int.TryParse(paramCommandName.ToString(), out int paramValue))
            {
                return "Resin count must be in range of 0 to 160 (inclusive).";
            }
            return await Executor.SetResinAsync(paramValue);
        }
    }
}
