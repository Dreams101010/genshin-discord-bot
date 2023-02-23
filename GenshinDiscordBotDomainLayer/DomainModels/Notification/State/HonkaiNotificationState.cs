using GenshinDiscordBotCrawler.Honkai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.Notification.State
{
    public class HonkaiNotificationState
    {
        public IList<HonkaiPromoCodeData> PromoCodes { get; set; }

        public HonkaiNotificationState(IList<HonkaiPromoCodeData> promocodes)
        {
            PromoCodes = promocodes ?? throw new ArgumentNullException(nameof(promocodes));
        }
    }
}
