using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Interfaces.Services.Notification;
using Serilog;
using GenshinDiscordBotCrawler;
using System.Text.Json;
using GenshinDiscordBotDomainLayer.DomainModels.Notification.State;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System.Diagnostics;

namespace GenshinDiscordBotDomainLayer.Services.Notification
{
    public class GenshinPromocodeService : IGenshinPromocodeService
    {

        public ILogger Logger { get; }
        public INotificationDatabaseInteractionHandler DatabaseInteractionHandler { get; }
        public GenshinWikiPromoTableParser Parser { get; }
        public INotifier Notifier { get; }
        public IDateTimeProvider DateTimeProvider { get; }
        public GenshinPromocodeService(ILogger logger, 
            INotificationDatabaseInteractionHandler notificationDatabaseInteractionHandler,
            GenshinWikiPromoTableParser parser,
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
                Logger.Information("Starting Genshin Impact Promocodes notification job.");
                bool success = true;
                // deserialize the promocode list
                var stateFromDb = DeserializeStateOrCreateNew(job.DataJson);
                // get parsed results
                var parsedResult = Parser.Parse();
                // aggregate promocode and promolink data from DB and web
                // these will be used to update db
                var activePromocodes = AggregatePromocodes(stateFromDb.PromoCodes, parsedResult.CodeResults);
                var activePromolinks = AggregatePromolinks(stateFromDb.PromoLinks, parsedResult.LinkResults);
                // get the promocodes and promolinks not yet shown to user
                var promocodesToShow = GetDifference(activePromocodes, stateFromDb.PromoCodes);
                var promolinksToShow = GetDifference(activePromolinks, stateFromDb.PromoLinks);
                // notify users
                if (promocodesToShow.Any() || promolinksToShow.Any())
                {
                    // form message for users
                    string message = GetSuccessMessage(promocodesToShow, promolinksToShow);
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
                        Logger.Error($"Genshin wiki parsing error. Message: {i.Message}\nContext: {i.Context}");
                    }
                }
                // if notification has been successful, store acquired results in the database
                if (success)
                {
                    Logger.Information("Genshin Impact Promocodes job was successful. Storing new state into database...");
                    var newState = new GenshinNotificationState(activePromocodes, activePromolinks);
                    job.DataJson = JsonSerializer.Serialize(newState);
                    await DatabaseInteractionHandler.UpdateNotificationJobAsync(job);
                    Logger.Information("New state for Genshin Impact Promocodes job has been saved.");
                }
                else
                {
                    Logger.Information("Couldn't notify users about Genshin Impact promocodes. Perhaps the Internet connection or Discord is down?");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Exception has occured when performing a Genshin Impact Promocodes job");
            }
        }

        private GenshinNotificationState DeserializeStateOrCreateNew(string json)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<GenshinNotificationState>(json);
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

            static GenshinNotificationState New()
            {
                return new GenshinNotificationState(new List<GenshinPromoCodeData>(),
                    new List<GenshinPromoLinkData>());
            }
        }

        public List<GenshinPromoCodeData> AggregatePromocodes(IList<GenshinPromoCodeData> oldData, 
            IEnumerable<GenshinPromoCodeData> newData)
        {
            HashSet<GenshinPromoCodeData> set = new(oldData);
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

        public List<GenshinPromoLinkData> AggregatePromolinks(IList<GenshinPromoLinkData> oldData,
            IEnumerable<GenshinPromoLinkData> newData)
        {
            HashSet<GenshinPromoLinkData> set = new(oldData);
            foreach (var promolink in newData.Where((x) => x.Expired))
            {
                if (set.Contains(promolink))
                {
                    set.Remove(promolink);
                }
            }
            foreach (var promolink in newData.Where((x) => !x.Expired))
            {
                if (!set.Contains(promolink))
                {
                    set.Add(promolink);
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

        public string GetSuccessMessage(IEnumerable<GenshinPromoCodeData> codes, 
            IEnumerable<GenshinPromoLinkData> links)
        {
            // TODO: use localization
            StringBuilder s = new();
            if (codes.Any())
            {
                s.AppendLine("New Genshin promocodes:");
                foreach (var code in codes)
                {
                    s.AppendLine($"Code: {code.Code}, Server: {code.Server}");
                }
            }
            if (links.Any())
            {
                s.AppendLine("New Genshin promo links:");
                foreach (var link in links)
                {
                    s.AppendLine($"Promo link: {link.Url}, Server: {link.Server}");
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
                s.Append($"Errors were encoutered during parsing of Genshin Impact Wiki. " +
                    $"Time of event: {DateTimeProvider.GetDateTime()}");
            }
            return s.ToString();
        }
    }
}
