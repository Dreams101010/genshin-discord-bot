using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.Services;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class ReminderDispatcherService : IReminderDispatcherService
    {
        public ILifetimeScope Scope { get; }
        public IBotMessageSender BotMessageSender { get; }
        public DateTimeBusinessLogic DateTimeBusinessLogic { get; }

        public ReminderDispatcherService(ILifetimeScope scope, 
            IBotMessageSender botMessageSender,
            DateTimeBusinessLogic dateTimeBusinessLogic)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            BotMessageSender = botMessageSender 
                ?? throw new ArgumentNullException(nameof(botMessageSender));
            DateTimeBusinessLogic = dateTimeBusinessLogic 
                ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
        }

        public async Task DispatcherAsync(CancellationToken cancellation)
        {
            await Task.Delay(10000, cancellation); // initial delay
            while (true)
            {
                if (cancellation.IsCancellationRequested)
                {
                    break;
                }
                var currentTime = DateTimeBusinessLogic.GetCurrentUtcTimeAsUnixSeconds();
                using var scope = Scope.BeginLifetimeScope();
                var reminderService = scope.Resolve<IReminderService>();
                var expiredReminders = await reminderService.GetExpiredRemindersAsync(currentTime);
                foreach (var reminder in expiredReminders)
                {
                    if (currentTime - reminder.ReminderTime < 300)
                    {
                        await SendReminderAsync(reminder);
                    }
                    else
                    {
                        await SendDelayedReminderAsync(reminder);
                    }
                }
                await reminderService.UpdateExpiredRecurrentRemindersAsync(currentTime);
                await reminderService.RemoveExpiredNonRecurrentRemindersAsync(currentTime);
                await Task.Delay(30000, cancellation);
            }
        }

        private async Task SendReminderAsync(Reminder reminder)
        {
            MessageContext messageContext = new()
            {
                ChannelId = reminder.ChannelId,
                DiscordUserId = reminder.UserDiscordId,
                GuildId = reminder.GuildId,
                Message = reminder.Message,
            };
            await BotMessageSender.SendMessageAsync(messageContext);
        }

        private async Task SendDelayedReminderAsync(Reminder reminder)
        {
            MessageContext messageContext = new()
            {
                ChannelId = reminder.ChannelId,
                DiscordUserId = reminder.UserDiscordId,
                GuildId = reminder.GuildId,
                Message = @$"{GetSorryMessage(reminder.UserLocale)} {reminder.Message} {DateTimeBusinessLogic.GetLocalTimeFromUnixSeconds(reminder.ReminderTime)}",
            };
            await BotMessageSender.SendMessageAsync(messageContext);
        }

        private string GetSorryMessage(UserLocale locale)
        {
            string message = locale switch
            {
                UserLocale.enGB => @"Sorry, but I couldn't remind you in time. Perhaps it was because I was offline.
Here's the initial reminder:",
                UserLocale.ruRU => @"Прошу прощения, но я не смогла напомнить вам вовремя. Возможно это произошло потому что я была выключена.
Вот изначальное упоминание:",
                _ => throw new NotImplementedException("Invalid UserLocale enum state"),
            };
            return message;
        }
    }
}
