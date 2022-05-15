using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using Microsoft.Data.Sqlite;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers
{
    public abstract class SqliteDatabaseInteractionHandler : IDatabaseInteractionHandler
    {
        protected SqliteConnection Connection { get; set; }
        public SqliteDatabaseInteractionHandler(SqliteConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        // Param -> Task
        public async Task ExecuteInTransactionAsync<T>(Func<T, Task> func, T param)
        {
            await Connection.OpenAsync();
            var transaction = await Connection.BeginTransactionAsync();
            try
            {
                await func(param);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
                await Connection.CloseAsync();
            }
        }

        // No param -> Task
        public async Task ExecuteInTransactionAsync(Func<Task> func)
        {
            await Connection.OpenAsync();
            var transaction = await Connection.BeginTransactionAsync();
            try
            {
                await func();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
                await Connection.CloseAsync();
            }
        }

        // Param -> Task<T>
        public async Task<TRet> ExecuteInTransactionAsync<T, TRet>
            (Func<T, Task<TRet>> func, T param)
        {
            await Connection.OpenAsync();
            var transaction = await Connection.BeginTransactionAsync();
            try
            {
                var result = await func(param);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
                await Connection.CloseAsync();
            }
        }

        // No param -> Task<T>
        public async Task<TRet> ExecuteInTransactionAsync<TRet>(Func<Task<TRet>> func)
        {
            await Connection.OpenAsync();
            var transaction = await Connection.BeginTransactionAsync();
            try
            {
                var result = await func();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
                await Connection.CloseAsync();
            }
        }
    }
}
