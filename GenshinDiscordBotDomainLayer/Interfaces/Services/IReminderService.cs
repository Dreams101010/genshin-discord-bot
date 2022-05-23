using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services
{
    public interface IReminderService
    {
        public Task UpdateOrCreateArtifactReminderAsync(DiscordMessageContext messageContext);
    }
}
