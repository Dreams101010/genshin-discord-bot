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
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateReminderAsync(description, timeSpan);
            await ReplyAsync(response);
        }

        [Command("remindDate")]
        public async Task UpdateOrCreateReminderByDate(string description, string dateTime)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateReminderByDateAsync(description, dateTime);
            await ReplyAsync(response);
        }

        [Command("remindRecurrent")]
        public async Task UpdateOrCreateRecurrentReminder(string description, string interval)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateRecurrentReminderAsync(description, interval);
            await ReplyAsync(response);
        }

        [Command("remindRecurrent")]
        public async Task UpdateOrCreateRecurrentReminder(string description, string startDateTime, string interval)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateRecurrentReminderAsync(description, startDateTime, interval);
            await ReplyAsync(response);
        }

        [Command("remindArtifacts")]
        public async Task UpdateOrCreateArtifactReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateArtifactReminderAsync();
            await ReplyAsync(response);
        }

        [Command("remindArtifacts")]
        public async Task UpdateOrCreateArtifactReminder(string time)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateArtifactReminderWithCustomTimeAsync(time);
            await ReplyAsync(response);
        }

        [Command("cancelRemindArtifacts")]
        public async Task CancelArtifactRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .RemoveArtifactRemindersForUserAsync();
            await ReplyAsync(response);
        }

        [Command("remindCheckIn")]
        public async Task UpdateOrCreateCheckInReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateCheckInReminderAsync();
            await ReplyAsync(response);
        }

        [Command("remindCheckIn")]
        public async Task UpdateOrCreateCheckInReminder(string time)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateCheckInReminderWithCustomTimeAsync(time);
            await ReplyAsync(response);
        }

        [Command("cancelRemindCheckIn")]
        public async Task CancelCheckInRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .RemoveCheckInRemindersForUserAsync();
            await ReplyAsync(response);
        }

        [Command("remindTeapotPlantHarvest")]
        [Alias("remindHarvest")]
        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync();
            await ReplyAsync(response);
        }

        [Command("remindTeapotPlantHarvest")]
        [Alias("remindHarvest")]
        public async Task UpdateOrCreateSereniteaPotPlantHarvestReminder(int days, int hours)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(days, hours);
            await ReplyAsync(response);
        }

        [Command("cancelRemindTeapotPlantHarvest")]
        [Alias("cancelRemindHarvest")]
        public async Task CancelSereniteaPotPlantHarvestRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .RemoveSereniteaPotPlantHarvestRemindersForUserAsync();
            await ReplyAsync(response);
        }

        [Command("remindTransformer")]
        public async Task UpdateOrCreateParametricTransformerReminder()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateParametricTransformerReminderAsync();
            await ReplyAsync(response);
        }

        [Command("remindTransformer")]
        public async Task UpdateOrCreateParametricTransformerReminder(int days, int hours)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .UpdateOrCreateParametricTransformerReminderAsync(days, hours);
            await ReplyAsync(response);
        }

        [Command("cancelRemindTransformer")]
        public async Task CancelParametricTransformerRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor
                .RemoveParametricTransformerRemindersForUserAsync();
            await ReplyAsync(response);
        }

        [Command("listReminders")]
        [Alias("reminders")]
        public async Task PrintRemindersForUser()
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor.GetRemindersForUserAsync();
            await ReplyAsync(response);
        }

        [Command("cancelReminder")]
        public async Task RemoveReminderById(ulong reminderId)
        {
            using var scope = Scope.BeginLifetimeScope();
            var reminderCommandExecutor = scope.Resolve<ReminderCommandExecutor>();
            reminderCommandExecutor.PopulateContextAsync(Context);
            string response = await reminderCommandExecutor.RemoveReminderById(reminderId);
            await ReplyAsync(response);
        }
    }
}
