using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace GenshinDiscordBotDomainLayer.ErrorHandlers
{
    public class FacadeErrorHandler
    {
        private ILogger Logger { get; }
        public FacadeErrorHandler(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogException(Exception exception)
        {
            Logger.Error($"An exception has occured in command facade: {exception}");
        }
    }
}
