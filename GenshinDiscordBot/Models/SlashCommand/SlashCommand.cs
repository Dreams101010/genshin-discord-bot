using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.Models.SlashCommand
{
    public class SlashCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<SlashCommandParameter> Params { get; set; } = new List<SlashCommandParameter>();
    }
}
