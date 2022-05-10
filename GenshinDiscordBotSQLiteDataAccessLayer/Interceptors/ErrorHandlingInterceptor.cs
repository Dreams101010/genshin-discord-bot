using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using GenshinDiscordBotDomainLayer.Exceptions;
using Serilog;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Interceptors
{
    public class ErrorHandlingInterceptor : IInterceptor
    {
        private ILogger Logger { get; }

        public ErrorHandlingInterceptor(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while executing command: {e}");
                throw new DatabaseInteractionException(e.Message);
            }
        }
    }
}
