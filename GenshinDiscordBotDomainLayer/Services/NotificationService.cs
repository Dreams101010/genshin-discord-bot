using Autofac;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Interfaces.Services.Notification;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Services
{
    public class NotificationService : INotificationService
    {
        private Timer ExecutionTimer { get; set; }
        private IDateTimeProvider DateTimeProvider { get; }
        private ILifetimeScope Scope { get; }
        public ILogger Logger { get; }
        public NotificationService(IDateTimeProvider dateTimeProvider,
            ILifetimeScope scope, ILogger logger)
        {
            ExecutionTimer = new Timer(OnElapsed);
            DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start()
        {
            // run at start with 10 second delay, then run at interval set by GetStartDelay
            var startDelayMs = 10000;
            ExecutionTimer.Change(startDelayMs, Timeout.Infinite);
        }

        public void Stop()
        {
            ExecutionTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private async void OnElapsed(object? state)
        {
            try
            {
                Logger.Information("Performing notification jobs...");
                using var newScope = Scope.BeginLifetimeScope();
                var businessLogic = newScope.Resolve<IGenshinPromocodeService>();
                var databaseHandler = newScope.Resolve<INotificationDatabaseInteractionHandler>();
                var jobs = await databaseHandler.GetNotificationJobsAsync();
                await PerformJobsAsync(jobs, newScope);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Exception has occured in Notification Service.");
            }
            finally
            {
                // reset the timer
                var nextDelay = GetStartDelay();
                ExecutionTimer.Change(nextDelay, Timeout.Infinite);
            }
        }

        private async Task PerformJobsAsync(IList<NotificationJob> jobs, ILifetimeScope scope)
        {
            foreach (var job in jobs)
            {
                switch (job.Kind)
                {
                    case NotificationJobKind.GenshinPromocodes:
                        {
                            var promocodeService = scope.Resolve<IGenshinPromocodeService>();
                            await promocodeService.PerformJobAsync(job);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("Unhandled enum state");
                        }
                }
            }
        }

        private int GetStartDelay()
        {
            var now = DateTimeProvider.GetDateTime();
            DateTime startTime;
            if (now.Hour >= 4)
            {
                if (now.Hour >= 16)
                {
                    var nextDay = now + TimeSpan.FromDays(1);
                    startTime = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day,
                        4, 0, 0);
                }
                else
                {
                    startTime = new DateTime(now.Year, now.Month, now.Day,
                        16, 0, 0);
                }
            }
            else
            {
                startTime = new DateTime(now.Year, now.Month, now.Day,
                    4, 0, 0);
            }
            var diff = startTime - now;
            return (int)diff.TotalMilliseconds;
        }
    }
}
