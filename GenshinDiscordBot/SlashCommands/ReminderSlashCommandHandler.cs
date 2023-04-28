using Discord.WebSocket;
using GenshinDiscordBotUI.CommandExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.SlashCommands
{
    public class ReminderSlashCommandHandler
    {
        ReminderCommandExecutor Executor { get; set; }

        public ReminderSlashCommandHandler(ReminderCommandExecutor executor)
        {
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public async Task<string> HandleAsync(SocketSlashCommand command)
        {
            Executor.PopulateContextAsync(command);
            string response = command.CommandName switch
            {
                "remind" => await HandleRemindCommandAsync(command),
                "remind_date" => await HandleRemindDateCommandAsync(command),
                "remind_recurrent" => await HandleRemindRecurrentCommandAsync(command),
                "remind_recurrent_date_time" 
                    => await HandleRemindRecurrentDateTimeCommandAsync(command),
                "remind_artifacts" => await Executor.UpdateOrCreateArtifactReminderAsync(),
                "remind_artifacts_time" => await HandleRemindArtifactsTimeCommandAsync(command),
                "cancel_remind_artifacts" => await Executor.RemoveArtifactRemindersForUserAsync(),
                "remind_checkin" => await Executor.UpdateOrCreateCheckInReminderAsync(),
                "remind_checkin_time" => await HandleRemindCheckInTimeCommandAsync(command),
                "cancel_remind_checkin" => await Executor.RemoveCheckInRemindersForUserAsync(),
                "remind_harvest" => await Executor.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(),
                "remind_harvest_time" => await HandleRemindHarvestTimeCommandAsync(command),
                "cancel_remind_harvest" => await Executor.RemoveSereniteaPotPlantHarvestRemindersForUserAsync(),
                "remind_transformer" => await Executor.UpdateOrCreateParametricTransformerReminderAsync(),
                "remind_transformer_time" => await HandleRemindTransformerTimeCommandAsync(command),
                "cancel_remind_transformer" => await Executor.RemoveParametricTransformerRemindersForUserAsync(),
                "list_reminders" => await Executor.GetRemindersForUserAsync(),
                "cancel_reminder" => await HandleCancelReminderCommandAsync(command),
                _ => throw new NotImplementedException("Unknown slash command in category: user"),
            };
            return response;
        }

        private async Task<string> HandleRemindCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandMessage = command.Data.Options.Where(x => x.Name == "message").First().Value as string;
            if (paramCommandMessage == null)
            {
                throw new Exception("Required parameter was null: message");
            }
            var paramCommandTimeTo = command.Data.Options.Where(x => x.Name == "time_to").First().Value as string;
            if (paramCommandTimeTo == null)
            {
                throw new Exception("Required parameter was null: time_to");
            }
            return await Executor.UpdateOrCreateReminderAsync(paramCommandMessage, paramCommandTimeTo);
        }

        private async Task<string> HandleRemindDateCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_date")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandMessage = command.Data.Options.Where(x => x.Name == "message").First().Value as string;
            if (paramCommandMessage == null)
            {
                throw new Exception("Required parameter was null: message");
            }
            var paramCommandDateTime = command.Data.Options.Where(x => x.Name == "date_and_time").First().Value as string;
            if (paramCommandDateTime == null)
            {
                throw new Exception("Required parameter was null: date_and_time");
            }
            return await Executor.UpdateOrCreateReminderByDateAsync(paramCommandMessage, paramCommandDateTime);
        }

        private async Task<string> HandleRemindRecurrentCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_recurrent")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandMessage = command.Data.Options.Where(x => x.Name == "message").First().Value as string;
            if (paramCommandMessage == null)
            {
                throw new Exception("Required parameter was null: message");
            }
            var paramCommandTimeBetween = command.Data.Options.Where(x => x.Name == "time_between").First().Value as string;
            if (paramCommandTimeBetween == null)
            {
                throw new Exception("Required parameter was null: date_and_time");
            }
            return await Executor.UpdateOrCreateRecurrentReminderAsync(paramCommandMessage, paramCommandTimeBetween);
        }
        private async Task<string> HandleRemindRecurrentDateTimeCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_recurrent_date_time")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandMessage = command.Data.Options.Where(x => x.Name == "message").First().Value as string;
            if (paramCommandMessage == null)
            {
                throw new Exception("Required parameter was null: message");
            }
            var paramCommandDateTime = command.Data.Options.Where(x => x.Name == "first_date_time").First().Value as string;
            if (paramCommandDateTime == null)
            {
                throw new Exception("Required parameter was null: first_date_time");
            }
            var paramCommandTimeBetween = command.Data.Options.Where(x => x.Name == "time_between").First().Value as string;
            if (paramCommandTimeBetween == null)
            {
                throw new Exception("Required parameter was null: date_and_time");
            }
            return await Executor.UpdateOrCreateRecurrentReminderAsync(paramCommandMessage, 
                paramCommandDateTime, paramCommandTimeBetween);
        }

        private async Task<string> HandleRemindArtifactsTimeCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_artifacts_time")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandTime = command.Data.Options.Where(x => x.Name == "time").First().Value as string;
            if (paramCommandTime == null)
            {
                throw new Exception("Required parameter was null: time");
            }
            return await Executor.UpdateOrCreateArtifactReminderWithCustomTimeAsync(paramCommandTime);
        }

        private async Task<string> HandleRemindCheckInTimeCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_checkin_time")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandTime = command.Data.Options.Where(x => x.Name == "time").First().Value as string;
            if (paramCommandTime == null)
            {
                throw new Exception("Required parameter was null: time");
            }
            return await Executor.UpdateOrCreateCheckInReminderWithCustomTimeAsync(paramCommandTime);
        }

        private async Task<string> HandleRemindHarvestTimeCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_harvest_time")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandDays = command.Data.Options.Where(x => x.Name == "days").First().Value;
            if (paramCommandDays == null)
            {
                throw new Exception("Required parameter was null: days");
            }
            if (!int.TryParse(paramCommandDays.ToString(), out int paramDaysValue))
            {
                return "Number of days must be non-negative and not exceed 2.";
            }
            var paramCommandHours = command.Data.Options.Where(x => x.Name == "hours").First().Value;
            if (paramCommandDays == null)
            {
                throw new Exception("Required parameter was null: hours");
            }
            if (!int.TryParse(paramCommandHours.ToString(), out int paramHoursValue))
            {
                return "Number of hours must be non-negative and not exceed 23.";
            }
            return await Executor.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(paramDaysValue, paramHoursValue);
        }

        private async Task<string> HandleRemindTransformerTimeCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "remind_transformer_time")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandDays = command.Data.Options.Where(x => x.Name == "days").First().Value;
            if (paramCommandDays == null)
            {
                throw new Exception("Required parameter was null: days");
            }
            if (!int.TryParse(paramCommandDays.ToString(), out int paramDaysValue))
            {
                return "Number of days must be non-negative and not exceed 7.";
            }
            var paramCommandHours = command.Data.Options.Where(x => x.Name == "hours").First().Value;
            if (paramCommandDays == null)
            {
                throw new Exception("Required parameter was null: hours");
            }
            if (!int.TryParse(paramCommandHours.ToString(), out int paramHoursValue))
            {
                return "Number of hours must be non-negative and not exceed 23.";
            }
            return await Executor.UpdateOrCreateParametricTransformerReminderAsync(paramDaysValue, paramHoursValue);
        }

        private async Task<string> HandleCancelReminderCommandAsync(SocketSlashCommand command)
        {
            if (command.CommandName != "cancel_reminder")
            {
                throw new ArgumentException($"Invalid command: {command.CommandName}");
            }
            var paramCommandName = command.Data.Options.Where(x => x.Name == "id").First().Value;
            if (paramCommandName == null)
            {
                throw new Exception("Required parameter was null: id");
            }
            if (!ulong.TryParse(paramCommandName.ToString(), out ulong paramValue))
            {
                return "Id must be positive and valid.";
            }
            return await Executor.RemoveReminderById(paramValue);
        }
    }
}
