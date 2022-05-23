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
    }
}
