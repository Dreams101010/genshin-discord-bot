using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ResultModels
{
    public struct TimeToResin
    {
        public DateTime TimeToResinUtc { get; set; }
        public TimeSpan TimeSpanToResin { get; set; }
    }
    public struct ResinInfoResultModel
    {
        public int CurrentCount { get; set; }
        public TimeSpan TimeToFullResin { get; set; }
        public Dictionary<int, TimeToResin> CompletionTimes { get; set; }
    }
}
