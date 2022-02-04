using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using Serilog;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public class DatabaseInitializer
    {
        ILogger Logger { get; set; }
        SQLiteConnectionProvider ConnectionProvider { get; set; }
        public DatabaseInitializer(ILogger logger,
            SQLiteConnectionProvider dbConnectionProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ConnectionProvider = dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        void InitializeTableStructure()
        {
            SqliteConnection conn = ConnectionProvider.GetConnection();
            conn.Open();
            using var command = new SqliteCommand(null, conn);
            command.CommandText = @"CREATE TABLE IF NOT EXISTS users
            (
	            discord_user_id numeric(20),
	            user_location text NOT NULL,
	            user_locale text NOT NULL,
	            CONSTRAINT users_discord_user_id_pkey PRIMARY KEY (discord_user_id),
	            CONSTRAINT user_location_valid CHECK (user_location IN ('Not specified','Moscow, Russia', 'Saint Petersburg, Russia', 'London, Great Britain')),
                CONSTRAINT user_locale_valid CHECK (user_locale IN ('ru-RU', 'en-GB'))
            );
            ";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS resin_tracking
            (
	            user_discord_id numeric(20),
	            init_time text NOT NULL,
	            resin_count smallint NOT NULL,
	            PRIMARY KEY(user_discord_id),
 	            FOREIGN KEY (user_discord_id) REFERENCES users(discord_user_id),
	            CONSTRAINT resin_count_valid CHECK (resin_count >= 0 AND resin_count <= 160)
            );";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS artifact_tracking
            (
	            id text,
	            user_discord_id numeric(20),
	            init_time text NOT NULL,
	            description text NOT NULL,
	            UNIQUE (user_discord_id, description),
	            PRIMARY KEY(id),
 	            FOREIGN KEY (user_discord_id) REFERENCES users(discord_user_id)
            );";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS pity_tracking
            (
	            user_discord_id numeric(20),
	            banner text NOT NULL,
	            rolls_since_four_star smallint,
	            rolls_since_five_star smallint,
	            PRIMARY KEY (user_discord_id),
	            FOREIGN KEY (user_discord_id) REFERENCES users (discord_user_id),
	            CONSTRAINT banner_valid CHECK (banner IN ('standard', 'weapon', 'event')),
	            CONSTRAINT rolls_since_four_star_valid CHECK (rolls_since_four_star >= 0 AND rolls_since_four_star <= 200),
	            CONSTRAINT rolls_since_five_star_valid CHECK (rolls_since_five_star >= 0 AND rolls_since_five_star <= 200)
            );";
            command.ExecuteNonQuery();
            conn.Close();
        }

        public void InitializeDb()
        {
            try
            {
                InitializeTableStructure();
            }
            catch (Exception e)
            {
                Logger.Error($"Error while trying to initialize SQLite database : {e}");
                throw;
            }
        }
    }
}
