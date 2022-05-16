using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotDomainLayer.ErrorHandlers
{
    public class LoggingErrorHandler : IErrorHandler
    {
        private ILogger Logger { get; }

        public LoggingErrorHandler(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void HandleExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception has occured: ", ex.ToString());
                throw;
            }
        }
        public async Task HandleExceptions(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception has occured: ", ex.ToString());
                throw;
            }
        }

        public async Task<T> HandleExceptions<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception has occured: ", ex.ToString());
                throw;
            }
        }
    }
}
