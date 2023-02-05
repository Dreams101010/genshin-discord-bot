using System;
using System.Collections.Generic;
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
    }
}
