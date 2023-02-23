using GenshinDiscordBotCrawler.Genshin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.Notification.State
{
    public class GenshinNotificationState
    {
        public IList<GenshinPromoCodeData> PromoCodes { get; set; }
        public IList<GenshinPromoLinkData> PromoLinks { get; set; }

        public GenshinNotificationState(IList<GenshinPromoCodeData> promocodes,
            IList<GenshinPromoLinkData> promolinks)
        {
            PromoCodes = promocodes ?? throw new ArgumentNullException(nameof(promocodes));
            PromoLinks = promolinks ?? throw new ArgumentNullException(nameof(promolinks));
        }
    }
}
