using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Castle.DynamicProxy;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.ErrorHandlers;

namespace GenshinDiscordBotDomainLayer.Interceptors
{
    public class ErrorHandlingInterceptor : IInterceptor
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        // This value will not be null. It is used to create a properly typed version
        // of HandleAsyncWithResult<T> method
        private static readonly MethodInfo handleAsyncMethodInfo = typeof(ErrorHandlingInterceptor)
            .GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore CS8601 // Possible null reference assignment.

        public LoggingErrorHandler ErrorHandler { get; }

        public ErrorHandlingInterceptor(LoggingErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }
        public void Intercept(IInvocation invocation)
        {

            var delegateType = GetDelegateType(invocation);
            if (delegateType == MethodType.Synchronous)
            {
                ErrorHandler.HandleExceptions(() => invocation.Proceed());
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
            await ErrorHandler.HandleExceptions(async () => await task);
        }

        private async Task<T> HandleAsyncWithResult<T>(Task<T> task, IInvocation invocation)
        {
            var result = await ErrorHandler.HandleExceptions(async () => await task);
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
