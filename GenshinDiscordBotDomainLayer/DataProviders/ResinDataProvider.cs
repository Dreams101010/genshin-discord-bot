using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DataProviders
{
    public class ResinDataProvider
    {
        public int MinResin { get; private set; } = 0;
        public int MaxResin { get; private set; } = 160;
        public int MinutesPerOneResin { get; private set; } = 8;
    }
}
