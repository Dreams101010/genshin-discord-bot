using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Autofac;
using Serilog;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.CommandExecutors;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotUI.CommandModules
{
    public class ReminderCommandModule : ModuleBase<SocketCommandContext>
    {
        private ILifetimeScope Scope { get; }

        public ReminderCommandModule(ILifetimeScope scope) : base()
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
        [Command("remindArtifacts")]
        public async Task UpdateOrCreateArtifactReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateArtifactReminderAsync(context);
            await ReplyAsync(response);
        }

        [Command("cancelRemindArtifacts")]
        public async Task CancelArtifactRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveArtifactRemindersForUserAsync(context);
            await ReplyAsync(response);
        }

        [Command("remindCheckIn")]
        public async Task UpdateOrCreateCheckInReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateCheckInReminderAsync(context);
            await ReplyAsync(response);
        }

        [Command("cancelRemindCheckIn")]
        public async Task CancelCheckInRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveCheckInRemindersForUserAsync(context);
            await ReplyAsync(response);
        }

        [Command("remindTeapotPlantHarvest")]
        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(context);
            await ReplyAsync(response);
        }

        [Command("cancelRemindTeapotPlantHarvest")]
        public async Task CancelSereniteaPotPlantHarvestRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveSereniteaPotPlantHarvestRemindersForUserAsync(context);
            await ReplyAsync(response);
        }

        [Command("listReminders")]
        [Alias("reminders")]
        public async Task PrintRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var id = Context.Message.Author.Id;
            string response = await reminderCommandExecutor
                .GetRemindersForUserAsync(id);
            await ReplyAsync(response);
        }
    }
}
