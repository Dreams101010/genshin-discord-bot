﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Parameters.Query
{
    public struct GetUserByDiscordIdQueryParam
    {
        public ulong DiscordId { get; set; }
    }
}
