using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Exceptions;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public abstract class SQLiteAbstractQuery<TParam, TResult> : IQuery<TParam, TResult>
    {
        private SQLiteConnectionProvider ConnectionProvider { get; }
        private ILogger Logger { get; }

        public SQLiteAbstractQuery(SQLiteConnectionProvider connectionProvider,
            ILogger loggger)
        {
            ConnectionProvider = connectionProvider
                ?? throw new ArgumentNullException(nameof(connectionProvider));
            Logger = loggger ?? throw new ArgumentNullException(nameof(loggger));
        }

        public async Task<TResult> QueryAsync(TParam param, bool retry = true)
        {
            Logger.Information($"In query with param of type {typeof(TParam)}");
            return await ErrorHandlingDecoratorAsync(param, retry);
        }

        private async Task<TResult> ErrorHandlingDecoratorAsync(TParam param, bool retry)
        {
            try
            {
                return await RetryDecoratorAsync(param, retry);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while executing query: {e}");
                throw new DatabaseInteractionException(e.Message);
            }
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
                        var res = await PayloadAsync(param);
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
                return await PayloadAsync(param);
            }
        }

        protected abstract Task<TResult> PayloadAsync(TParam param);
    }
}
