using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Serilog;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class ReminderDispatcherService : IReminderDispatcherService
    {
        public ILifetimeScope Scope { get; }
        public IBotMessageSender BotMessageSender { get; }
        public DateTimeBusinessLogic DateTimeBusinessLogic { get; }
        public Localization.Localization Localization { get; }
        public ILogger Logger { get; }

        public ReminderDispatcherService(ILifetimeScope scope, 
            IBotMessageSender botMessageSender,
            DateTimeBusinessLogic dateTimeBusinessLogic,
            Localization.Localization localization,
            ILogger logger)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            BotMessageSender = botMessageSender 
                ?? throw new ArgumentNullException(nameof(botMessageSender));
            DateTimeBusinessLogic = dateTimeBusinessLogic 
                ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
            Localization = localization 
                ?? throw new ArgumentNullException(nameof(localization));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DispatcherAsync(CancellationToken cancellation)
        {
            try
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
                    bool successOnEveryMessage = true;
                    foreach (var reminder in expiredReminders)
                    {
                        bool sendSuccessful = true;
                        if (currentTime - reminder.ReminderTime < 300)
                        {
                            sendSuccessful = await SendReminderAsync(reminder);
                        }
                        else
                        {
                            sendSuccessful = await SendDelayedReminderAsync(reminder);
                        }
                        if (!sendSuccessful)
                        {
                            successOnEveryMessage = false;
                        }
                    }
                    // only update and remove reminders if we have successfully sent
                    // all messages
                    if (expiredReminders.Count > 0 && successOnEveryMessage)
                    {
                        await reminderService.UpdateExpiredRecurrentRemindersAsync(currentTime);
                        await reminderService.RemoveExpiredNonRecurrentRemindersAsync(currentTime);
                    }
                    await Task.Delay(120000, cancellation);
                }
            }
            catch (OperationCanceledException) { }
        }

        private async Task<bool> SendReminderAsync(Reminder reminder)
        {
            MessageContext messageContext = new()
            {
                ChannelId = reminder.ChannelId,
                DiscordUserId = reminder.UserDiscordId,
                GuildId = reminder.GuildId,
                Message = reminder.Message,
            };
            return await BotMessageSender.SendMessageAsync(messageContext);
        }

        private async Task<bool> SendDelayedReminderAsync(Reminder reminder)
        {
            MessageContext messageContext = new()
            {
                ChannelId = reminder.ChannelId,
                DiscordUserId = reminder.UserDiscordId,
                GuildId = reminder.GuildId,
                Message = @$"{GetSorryMessage(reminder.UserLocale)} {reminder.Message} {DateTimeBusinessLogic.GetLocalTimeFromUnixSeconds(reminder.ReminderTime)}",
            };
            return await BotMessageSender.SendMessageAsync(messageContext);
        }
        
        private string GetSorryMessage(UserLocale locale)
        {
            return Localization.GetLocalizedString("Reminder", "ReminderSorryMessage", locale);
        }
    }
}
