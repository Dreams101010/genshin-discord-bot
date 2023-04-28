using GenshinDiscordBotCrawler.HonkaiStarRail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels.Notification.State
{
    public class StarRailNotificationState
    {
        public IList<StarRailPromoCodeData> PromoCodes { get; set; }

        public StarRailNotificationState(IList<StarRailPromoCodeData> promocodes)
        {
            PromoCodes = promocodes ?? throw new ArgumentNullException(nameof(promocodes));
        }
    }
}
