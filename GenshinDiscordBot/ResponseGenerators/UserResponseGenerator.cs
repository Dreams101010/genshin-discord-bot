using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class UserResponseGenerator
    {
        public IDateTimeProvider DateTimeProvider { get; }
        public UserResponseGenerator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider 
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public string GetUserSettingsList(User user)
        {
            return string.Format("Locale: {0}, Location: {1}", user.Locale, user.Location);
        }

        public string GetGeneralErrorMessage()
        {
            var nowUtc = DateTimeProvider.GetDateTime().ToUniversalTime();
            return string.Format(@"Something went wrong. 
								Please contact the developer. 
								The time of the event: {0}", nowUtc);
        }

        public string GetListOfPossibleLocales()
        {
            return "Possible locales are: \n" +
                "ruRU \n" +
                "enGB \n";
        }

        public string GetListOfPossibleLocations()
        {
            return "Possible locations are: \n" +
                "\"Not specified\" \n" +
                "\"Moscow, Russia\" \n" +
                "\"Saint Petersburg, Russia\" \n" +
                "\"London, Great Britain\"";
        }

        public string GetLocaleErrorMessage()
        {
            return "Incorrect locale setting. Correct settings are ruRU and enGB";
        }

        public string GetLocaleSuccessMessage()
        {
            return "Locale has been set.";
        }

        public string GetLocationErrorMessage()
        {
            return "Incorrect location setting. Correct settings are: \n" +
                        "Not specified \n" +
                        "Moscow, Russia \n" +
                        "Saint Petersburg, Russia \n" +
                        "London, Great Britain";
        }

        public string GetLocationSuccessMessage()
        {
            return "Location has been set.";
        }
    }
}
