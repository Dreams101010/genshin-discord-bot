using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Serilog;
using Dapper;
using GenshinDiscordBotDomainLayer.BusinessLogic;

namespace GenshinDiscordBotSQLiteDataAccessLayer
{
    public class DatabaseInitializer
    {
        private ILogger Logger { get; }
        private SqliteConnection Connection { get; }
        private DateTimeBusinessLogic DateTimeBusinessLogic { get; }

        public DatabaseInitializer(ILogger logger,
            SqliteConnection connection,
            DateTimeBusinessLogic dateTimeBusinessLogic)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            DateTimeBusinessLogic = dateTimeBusinessLogic ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
        }

        void InitializeTableStructure()
        {
            Connection.Open();
            using var command = new SqliteCommand(null, Connection);
            command.CommandText = @"CREATE TABLE IF NOT EXISTS users
                (
	                discord_user_id numeric(20),
	                user_locale text NOT NULL,
	                reminders_opt_in boolean NOT NULL DEFAULT false,
	                CONSTRAINT users_discord_user_id_pkey PRIMARY KEY (discord_user_id),
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
            command.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='reminder_categories';";
            long? nullableCount = (long?)command.ExecuteScalar();
            if (!nullableCount.HasValue)
            {
                throw new Exception("Database query returned no results");
            }
            long count = nullableCount.Value;
            if (count == 0)
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS reminder_categories
                (
	                id integer PRIMARY KEY,
	                name text UNIQUE NOT NULL
                );";
                command.ExecuteNonQuery();
                command.CommandText = @"INSERT INTO reminder_categories (name) VALUES 
                ('Artifact reminder'),('Check-in reminder'),('Serenitea pot plant harvest'),('Generic reminder'),('Parametric Transformer reminder')";
                command.ExecuteNonQuery();
            }
            command.CommandText = @"CREATE TABLE IF NOT EXISTS reminders 
                (
	                id integer PRIMARY KEY,
	                guild_id numeric(20) NOT NULL,
	                channel_id numeric(20) NOT NULL, 
	                user_discord_id numeric(20) NOT NULL,
	                interval numeric(20) NOT NULL,
	                reminder_time numeric(20) NOT NULL,
	                category_id integer NOT NULL, 
	                message text NOT NULL,
	                recurrent boolean NOT NULL,
 	                FOREIGN KEY (user_discord_id) REFERENCES users(discord_user_id),
	                FOREIGN KEY (category_id) REFERENCES reminder_categories(id)
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
            // Fast-forward delayed reminders
            var fastForwardSql = @"
                UPDATE reminders 
                SET reminder_time = reminder_time + round(((@CurrentTime - reminder_time) / interval) - 0.5)  * interval 
                WHERE reminder_time + interval < @CurrentTime AND recurrent = true
            ";
            Connection.Execute(fastForwardSql, new 
                { 
                    CurrentTime = DateTimeBusinessLogic.GetCurrentUtcTimeAsUnixSeconds()
                }
            );
            Connection.Close();
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
