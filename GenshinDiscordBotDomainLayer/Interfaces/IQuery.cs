using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface IQuery<TParam, TResult>
    {
        public Task<TResult> QueryAsync(TParam param, bool retry);
    }
}
