using Autofac;
using Discord;
using Discord.WebSocket;
using GenshinDiscordBotUI.CommandExecutors;
using GenshinDiscordBotUI.Models.SlashCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.SlashCommands
{
    public class SlashCommandDispatcher
    {
        private SlashCommandSource SlashCommandSource { get; }
        private ILifetimeScope Scope { get; }

        public SlashCommandDispatcher(SlashCommandSource slashCommandSource, ILifetimeScope scope)
        {
            SlashCommandSource = slashCommandSource
                ?? throw new ArgumentNullException(nameof(slashCommandSource));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        private ApplicationCommandOptionType GetParameterType(SlashCommandParameter parameter)
        {
            var result = parameter.Type switch
            {
                "String" => ApplicationCommandOptionType.String,
                "Int" => ApplicationCommandOptionType.Integer,
                "Double" => ApplicationCommandOptionType.Number,
                "Boolean" => ApplicationCommandOptionType.Boolean,
                _ => throw new NotImplementedException("Invalid enum state"),
            };
            return result;
        }

        public async Task RegisterSlashCommandsAsync(DiscordSocketClient client)
        {
            try
            {
                List<ApplicationCommandProperties> applicationCommandProperties = new();
                foreach (var command in SlashCommandSource.Commands.Values)
                {
                    var slashCommandBuilder = new Discord.SlashCommandBuilder()
                        .WithName(command.Name)
                        .WithDescription(command.Description);
                    foreach (var param in command.Params)
                    {
                        slashCommandBuilder.AddOption(param.Name, GetParameterType(param),
                            param.Description, param.IsRequired);
                    }
                    applicationCommandProperties.Add(slashCommandBuilder.Build());
                }
                await client.Rest.BulkOverwriteGlobalCommands(applicationCommandProperties.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DispatchAsync(SocketSlashCommand command)
        {
            using var scope = Scope.BeginLifetimeScope();
            switch (SlashCommandSource.Commands[command.CommandName].Category)
            {
                case "User":
                    var userSlashCommandHandler = scope.Resolve<UserSlashCommandHandler>();
                    return await userSlashCommandHandler.HandleAsync(command);
                case "Resin":
                    var resinSlashCommandHandler = scope.Resolve<ResinSlashCommandHandler>();
                    return await resinSlashCommandHandler.HandleAsync(command);
                case "Reminder":
                    var reminderSlashCommandHandler = scope.Resolve<ReminderSlashCommandHandler>();
                    return await reminderSlashCommandHandler.HandleAsync(command);
                default:
                    throw new NotImplementedException("Unknown category");
            }
        }
    }
}
