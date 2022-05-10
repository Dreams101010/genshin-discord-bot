using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Castle.DynamicProxy;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Interceptors
{
    public class RetryInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    invocation.Proceed();
                    break;
                }
                catch
                {
                    if (retryCount < 5)
                    {
                        retryCount++;
                        Thread.Sleep(retryCount * 100);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
