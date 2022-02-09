using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class GeneralResponseGenerator
    {
        public IDateTimeProvider DateTimeProvider { get; }
        public GeneralResponseGenerator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public string GetGeneralErrorMessage()
        {
            var nowUtc = DateTimeProvider.GetDateTime().ToUniversalTime();
            return string.Format(@"Something went wrong. 
								Please contact the developer. 
								The time of the event: {0}", nowUtc);
        }
    }
}
