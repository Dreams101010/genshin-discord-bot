using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ResultModels
{
    public struct ResinInfoResultModel
    {
        public int CurrentCount { get; set; }
        public TimeSpan TimeToFullResin { get; set; }
        public DateTime CompletionTime { get; set; }
    }
}
