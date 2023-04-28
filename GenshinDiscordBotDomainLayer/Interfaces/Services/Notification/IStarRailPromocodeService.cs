using GenshinDiscordBotDomainLayer.DomainModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services.Notification
{
    public interface IStarRailPromocodeService
    {
        public Task PerformJobAsync(NotificationJob job);
    }
}
