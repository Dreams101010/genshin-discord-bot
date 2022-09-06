using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI
{
    internal class Application
    {
        public Bot Bot { get; }

        public Application(Bot bot)
        {
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public async Task StartApplication(CancellationToken token)
        {
            await Bot.StartBot(token);
        }
    }
}
