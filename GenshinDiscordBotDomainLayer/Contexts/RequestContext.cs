using GenshinDiscordBotDomainLayer.DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Contexts
{
    public class RequestContext
    {
        public DiscordContext DiscordContext { get; init; }
        public UserContext UserContext { get; init; }

        public RequestContext(DiscordContext discordContext, UserContext userContext)
        {
            DiscordContext = discordContext ?? throw new ArgumentNullException(nameof(discordContext));
            UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        public CultureInfo GetUserCulture()
        {
            var culture = UserContext.User.Locale switch
            {
                UserLocale.enGB => CultureInfo.GetCultureInfo("en-GB"),
                UserLocale.ruRU => CultureInfo.GetCultureInfo("ru-RU"),
                _ => throw new Exception("Invalid enum state"),
            };
            return culture;
        }
    }
}
