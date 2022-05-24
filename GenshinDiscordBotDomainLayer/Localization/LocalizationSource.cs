using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Localization
{
    public class LocalizationSource
    {
        public Dictionary<string, Dictionary<string, string>> English { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Russian { get; set; } = new();
    }
}
