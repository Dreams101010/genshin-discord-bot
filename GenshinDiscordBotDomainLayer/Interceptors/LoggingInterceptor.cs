using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Castle.DynamicProxy;
using Serilog;

namespace GenshinDiscordBotDomainLayer.Interceptors
{
    public class LoggingInterceptor : IInterceptor
    {
#pragma warning disable CS8601 // Possible null reference assignment. 
        // This value will not be null. It is used to create a properly typed version
        // of HandleAsyncWithResult<T> method
        private static readonly MethodInfo handleAsyncMethodInfo = typeof(LoggingInterceptor)
            .GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore CS8601 // Possible null reference assignment.

        public ILogger Logger { get; }

        public LoggingInterceptor(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Intercept(IInvocation invocation)
        {

            var delegateType = GetDelegateType(invocation);
            if (delegateType == MethodType.Synchronous)
            {
                Logger.Information("In {0}.{1}", invocation.Method.DeclaringType, invocation.Method.Name);
                invocation.Proceed();
            }
            if (delegateType == MethodType.AsyncAction)
            {
                invocation.Proceed();
                invocation.ReturnValue = HandleAsync((Task)invocation.ReturnValue, invocation);
            }
            if (delegateType == MethodType.AsyncFunction)
            {
                invocation.Proceed();
                ExecuteHandleAsyncWithResultUsingReflection(invocation);
            }
        }

        private void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation)
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = handleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation.ReturnValue, invocation });
        }

        private async Task HandleAsync(Task task, IInvocation invocation)
        {
            Logger.Information("In {0}.{1}", invocation.Method.DeclaringType, invocation.Method.Name);
            await task;
        }

        private async Task<T> HandleAsyncWithResult<T>(Task<T> task, IInvocation invocation)
        {
            Logger.Information("In {0}.{1}", invocation.Method.DeclaringType, invocation.Method.Name);
            var result = await task;
            return result;
        }

        private MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}
