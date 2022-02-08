using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Parameters.Command
{
    public struct AddOrUpdateResinInfoCommandParam
    {
        public ResinTrackingInfo ResinInfo { get; set; }
    }
}
