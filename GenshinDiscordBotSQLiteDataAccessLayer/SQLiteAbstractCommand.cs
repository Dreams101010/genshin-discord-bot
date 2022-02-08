using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Serilog;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public abstract class SQLiteAbstractCommand<TParam, TResult> : ICommand<TParam, TResult>
    {
        SQLiteConnectionProvider ConnectionProvider { get; }
        ILogger Logger { get; set; }

        public SQLiteAbstractCommand(SQLiteConnectionProvider connectionProvider, 
            ILogger loggger)
        {
            ConnectionProvider = connectionProvider 
                ?? throw new ArgumentNullException(nameof(connectionProvider));
            Logger = loggger ?? throw new ArgumentNullException(nameof(loggger));
        }

        public async Task<TResult> ExecuteAsync(TParam param, bool useTransaction = true)
        {
            Logger.Information($"In command with param of type {typeof(TParam)}");
            return await RetryDecoratorAsync(param, useTransaction);
        }

        private async Task<TResult> RetryDecoratorAsync(TParam param, bool useTransaction)
        {
            if (useTransaction)
            {
                int retryCount = 0;
                while (true)
                {
                    try
                    {
                        return await TransactionDecoratorAsync(param, useTransaction);
                    }
                    catch
                    {
                        if(retryCount < 5)
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
                return await TransactionDecoratorAsync(param, useTransaction);
            }
        }

        private async Task<TResult> TransactionDecoratorAsync(TParam param, bool useTransaction)
        {
            if (useTransaction)
            {
                var conn = ConnectionProvider.GetConnection();
                await conn.OpenAsync();
                var transaction = await conn.BeginTransactionAsync();
                try
                {
                    var result = await ErrorHandlingDecoratorAsync(param);
                    await transaction.CommitAsync();
                    await conn.CloseAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
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
                Logger.Error($"An error has occured while executing command: {e}");
                throw;
            }
        }

        protected abstract Task<TResult> PayloadAsync(TParam param);
    }
}
