using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IDatabaseInteractionHandler
    {
        protected Task ExecuteInTransactionAsync(Func<Task> func);
        protected Task ExecuteInTransactionAsync<T>(Func<T, Task> func, T param);
        protected Task<TRet> ExecuteInTransactionAsync<TRet>(Func<Task<TRet>> func);
        protected Task<TRet> ExecuteInTransactionAsync<T, TRet>(Func<T, Task<TRet>> func, T param);
    }
}
