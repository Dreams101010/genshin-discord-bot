using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Interceptors
{
    public class TransactionInterceptor : IInterceptor
    {
        private SQLiteConnectionProvider ConnectionProvider { get; }

        public TransactionInterceptor(SQLiteConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public void Intercept(IInvocation invocation)
        {
            var connection = ConnectionProvider.GetConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                invocation.Proceed();
                transaction.Commit();
                connection.Close();
            }
            catch
            {
               transaction.Rollback();
            }
        }
    }
}
