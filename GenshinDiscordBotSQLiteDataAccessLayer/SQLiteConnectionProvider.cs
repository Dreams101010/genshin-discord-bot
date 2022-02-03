using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public class SQLiteConnectionProvider : IDisposable
    {
        private bool disposedValue;

        private IConfigurationRoot Configuration { get; set; }
        private SqliteConnection Connection { get; set; } = null;

        public SQLiteConnectionProvider(IConfigurationRoot configRoot)
        {
            Configuration = configRoot ?? throw new ArgumentNullException(nameof(configRoot));
        }

        public SqliteConnection GetConnection()
        {
            if (Connection == null)
            {
                string connString = Configuration.GetConnectionString("SQLite");
                Connection = new SqliteConnection(connString);
            }
            return Connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
