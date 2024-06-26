﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers
{
    public interface IResinDatabaseInteractionHandler
    {
        public Task<bool> SetResinForUserAsync(ResinTrackingInfo resinInfo);

        public Task<ResinTrackingInfo> GetResinForUserAsync(ulong discordId);
    }
}
