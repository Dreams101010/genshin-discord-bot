using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.Models.SlashCommand
{
    public class SlashCommandSource
    {
        public IReadOnlyDictionary<string, SlashCommand> Commands { get; private set; }
        public SlashCommandSource(IReadOnlyDictionary<string, SlashCommand> commands)
        {
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }
    }
}
