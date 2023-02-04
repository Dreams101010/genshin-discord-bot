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

        [Command("remind")]
        public async Task UpdateOrCreateReminder(string description, string timeSpan)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateReminderAsync(context, description, userName, timeSpan);
            await ReplyAsync(response);
        }

        [Command("remindRecurrent")]
        public async Task UpdateOrCreateRecurrentReminder(string description, string interval)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateRecurrentReminderAsync(context, description, userName, interval);
            await ReplyAsync(response);
        }

        [Command("remindArtifacts")]
        public async Task UpdateOrCreateArtifactReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateArtifactReminderAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindArtifacts")]
        public async Task UpdateOrCreateArtifactReminder(string time)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateArtifactReminderWithCustomTimeAsync(context, time, userName);
            await ReplyAsync(response);
        }

        [Command("cancelRemindArtifacts")]
        public async Task CancelArtifactRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveArtifactRemindersForUserAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindCheckIn")]
        public async Task UpdateOrCreateCheckInReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateCheckInReminderAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindCheckIn")]
        public async Task UpdateOrCreateCheckInReminder(string time)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateCheckInReminderWithCustomTimeAsync(context, time, userName);
            await ReplyAsync(response);
        }

        [Command("cancelRemindCheckIn")]
        public async Task CancelCheckInRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveCheckInRemindersForUserAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindTeapotPlantHarvest")]
        [Alias("remindHarvest")]
        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindTeapotPlantHarvest")]
        [Alias("remindHarvest")]
        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminder(int days, int hours)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(context, days, hours, userName);
            await ReplyAsync(response);
        }

        [Command("cancelRemindTeapotPlantHarvest")]
        [Alias("cancelRemindHarvest")]
        public async Task CancelSereniteaPotPlantHarvestRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveSereniteaPotPlantHarvestRemindersForUserAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindTransformer")]
        public async Task UpdateOrCreateParametricTransformerReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateParametricTransformerReminderAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("remindTransformer")]
        public async Task UpdateOrCreateParametricTransformerReminder(int days, int hours)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .UpdateOrCreateParametricTransformerReminderAsync(context, days, hours, userName);
            await ReplyAsync(response);
        }

        [Command("cancelRemindTransformer")]
        public async Task CancelParametricTransformerRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            DiscordMessageContext context = new DiscordMessageContext()
            {
                UserDiscordId = Context.Message.Author.Id,
                ChannelId = Context.Message.Channel.Id,
                GuildId = Context.Guild.Id,
            };
            string response = await reminderCommandExecutor
                .RemoveParametricTransformerRemindersForUserAsync(context, userName);
            await ReplyAsync(response);
        }

        [Command("listReminders")]
        [Alias("reminders")]
        public async Task PrintRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userName = Context.Message.Author.Username;
            var id = Context.Message.Author.Id;
            var guildId = Context.Guild.Id;
            var channelId = Context.Channel.Id;
            string response = await reminderCommandExecutor
                .GetRemindersForUserAsync(id, guildId, channelId, userName);
            await ReplyAsync(response);
        }

        [Command("cancelReminder")]
        public async Task RemoveReminderById(ulong reminderId)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            var userId = Context.Message.Author.Id;
            string response = await reminderCommandExecutor
                .RemoveReminderById(userId, reminderId);
            await ReplyAsync(response);
        }
    }
}
