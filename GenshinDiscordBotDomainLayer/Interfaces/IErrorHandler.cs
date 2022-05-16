using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IErrorHandler
    {
        public void HandleExceptions(Action action);
        public Task HandleExceptions(Func<Task> func);
        public Task<T> HandleExceptions<T>(Func<Task<T>> func);
    }
}
