using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.Models.SlashCommand
{
    public class SlashCommandListJsonModel
    {
        public List<SlashCommand> Commands { get; set; } = new();

        public SlashCommandSource ToSlashCommandSource()
        {
            Dictionary<string, SlashCommand> result = new();
            foreach (var command in Commands)
            {
                result.Add(command.Name, command);
            }
            return new SlashCommandSource(result);
        }
    }
}
