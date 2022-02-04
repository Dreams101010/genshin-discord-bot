using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public abstract class SQLiteAbstractQuery<TParam, TResult> : IQuery<TParam, TResult>
    {
        SQLiteConnectionProvider ConnectionProvider { get; }
        ILogger Logger { get; set; }

        public SQLiteAbstractQuery(SQLiteConnectionProvider connectionProvider,
            ILogger loggger)
        {
            ConnectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
            Logger = loggger ?? throw new ArgumentNullException(nameof(loggger));
        }

        public async Task<TResult> QueryAsync(TParam param, bool retry = true)
        {
            return await RetryDecoratorAsync(param, retry);
        }

        private async Task<TResult> RetryDecoratorAsync(TParam param, bool retry)
        {
            if (retry)
            {
                int retryCount = 0;
                while (true)
                {
                    try
                    {
                        var conn = ConnectionProvider.GetConnection();
                        await conn.OpenAsync();
                        var res = await ErrorHandlingDecoratorAsync(param);
                        await conn.CloseAsync();
                        return res;
                    }
                    catch
                    {
                        if (retryCount < 5)
                        {
                            retryCount++;
                            await Task.Delay(retryCount * 100);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                return await ErrorHandlingDecoratorAsync(param);
            }
        }

        private async Task<TResult> ErrorHandlingDecoratorAsync(TParam param)
        {
            try
            {
                return await PayloadAsync(param);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while executing query: {e}");
                throw;
            }
        }

        protected abstract Task<TResult> PayloadAsync(TParam param);
    }
}
