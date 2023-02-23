using GenshinDiscordBotCrawler;
using GenshinDiscordBotCrawler.Honkai;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.Services.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using GenshinDiscordBotCrawler.Genshin;
using GenshinDiscordBotDomainLayer.DomainModels.Notification.State;
using System.Text.Json;

namespace GenshinDiscordBotDomainLayer.Services.Notification
{
    public class HonkaiPromocodeService : IHonkaiImpact3rdPromocodeService
    {
        public ILogger Logger { get; }
        public INotificationDatabaseInteractionHandler DatabaseInteractionHandler { get; }
        public HonkaiWikiPromoTableParser Parser { get; }
        public INotifier Notifier { get; }
        public IDateTimeProvider DateTimeProvider { get; }

        public HonkaiPromocodeService(ILogger logger,
            INotificationDatabaseInteractionHandler notificationDatabaseInteractionHandler,
            HonkaiWikiPromoTableParser parser,
            INotifier notifier,
            IDateTimeProvider dateTimeProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DatabaseInteractionHandler = notificationDatabaseInteractionHandler
                ?? throw new ArgumentNullException(nameof(notificationDatabaseInteractionHandler));
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
            Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task PerformJobAsync(NotificationJob job)
        {
            try
            {
                Logger.Information("Starting Honkai Impact 3rd Promocodes notification job.");
                bool success = true;
                // deserialize the promocode list
                var stateFromDb = DeserializeStateOrCreateNew(job.DataJson);
                // get parsed results
                var parsedResult = Parser.Parse();
                // aggregate promocode data from DB and web
                // these will be used to update db
                var activePromocodes = AggregatePromocodes(stateFromDb.PromoCodes, parsedResult.CodeResults);
                // get the promocodes not yet shown to user
                var promocodesToShow = GetDifference(activePromocodes, stateFromDb.PromoCodes);
                // notify users
                if (promocodesToShow.Any())
                {
                    // form message for users
                    string message = GetSuccessMessage(promocodesToShow);
                    // notify users
                    success &= await Notifier.Notify(message, job.SuccessEndpoint);
                }
                // notify about errors
                if (parsedResult.Errors.Any())
                {
                    // form message
                    string message = GetErrorMessage(parsedResult.Errors);
                    success &= await Notifier.Notify(message, job.ErrorEndpoint);
                    foreach (var i in parsedResult.Errors)
                    {
                        Logger.Error($"Honkai wiki parsing error. Message: {i.Message}\nContext: {i.Context}");
                    }
                }
                // if notification has been successful, store acquired results in the database
                if (success)
                {
                    Logger.Information("Honkai Impact 3rd Promocodes job was successful. Storing new state into database...");
                    var newState = new HonkaiNotificationState(activePromocodes);
                    job.DataJson = JsonSerializer.Serialize(newState);
                    await DatabaseInteractionHandler.UpdateNotificationJobAsync(job);
                    Logger.Information("New state for Honkai Impact 3rd Promocodes job has been saved.");
                }
                else
                {
                    Logger.Information("Couldn't notify users about Honkai Impact 3rd promocodes. Perhaps the Internet connection or Discord is down?");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Exception has occured when performing a Honkai Impact 3rd Promocodes job");
            }
        }

        private HonkaiNotificationState DeserializeStateOrCreateNew(string json)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<HonkaiNotificationState>(json);
                if (deserialized == null)
                {
                    return New();
                }
                return deserialized;
            }
            catch
            {
                return New();
            }

            static HonkaiNotificationState New()
            {
                return new HonkaiNotificationState(new List<HonkaiPromoCodeData>());
            }
        }

        public List<HonkaiPromoCodeData> AggregatePromocodes(IList<HonkaiPromoCodeData> oldData,
            IEnumerable<HonkaiPromoCodeData> newData)
        {
            HashSet<HonkaiPromoCodeData> set = new(oldData);
            foreach (var promocode in newData.Where((x) => x.Expired))
            {
                if (set.Contains(promocode))
                {
                    set.Remove(promocode);
                }
            }
            foreach (var promocode in newData.Where((x) => !x.Expired))
            {
                if (!set.Contains(promocode))
                {
                    set.Add(promocode);
                }
            }
            return set.ToList();
        }

        public IEnumerable<T> GetDifference<T>(IList<T> newData, IList<T> oldData)
        {
            HashSet<T> oldSet = new HashSet<T>(oldData);
            HashSet<T> newSet = new HashSet<T>(newData);
            return newSet.Except(oldSet);
        }

        public string GetSuccessMessage(IEnumerable<HonkaiPromoCodeData> codes)
        {
            // TODO: use localization
            StringBuilder s = new();
            if (codes.Any())
            {
                s.AppendLine("New Honkai Impact 3rd promocodes:");
                foreach (var code in codes)
                {
                    s.AppendLine($"Code: {code.Code}");
                }
            }
            return s.ToString();
        }

        public string GetErrorMessage(IEnumerable<ParseError> errors)
        {
            // TODO: use localization
            StringBuilder s = new();
            if (errors.Any())
            {
                s.Append($"Errors were encoutered during parsing of Honkai Impact 3rd Wiki. " +
                    $"Time of event: {DateTimeProvider.GetDateTime()}");
            }
            return s.ToString();
        }
    }
}
