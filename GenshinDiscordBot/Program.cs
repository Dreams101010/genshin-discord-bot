using System;
using System.Threading.Tasks;
using Autofac;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks;
using GenshinDiscordBotSQLiteDataAccessLayer;

namespace GenshinDiscordBotUI
{
    class Application
    {
        public Bot Bot { get; }

        public Application(Bot bot)
        {
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public async Task StartApplication()
        {
            await Bot.StartBot();
        }
    }
    class Program
    {
        // Logger for logging unhandled exceptions
        static ILogger Logger { get; set; } = null; 
        static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            // Application
            builder.RegisterType<Application>();
            // Configuration
            builder.Register(c => BuildConfigurationRoot()).InstancePerLifetimeScope();
            // Discord.NET
            builder.RegisterType<Bot>();
            builder.RegisterType<DiscordSocketClient>().InstancePerLifetimeScope();
            builder.Register(c => new CommandService()).InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().InstancePerLifetimeScope();
            // Logger
            builder.Register<ILogger>(
                c => new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).SingleInstance();
            // DAL
            builder.RegisterType<DatabaseInitializer>().SingleInstance();
            return builder.Build();
        }

        static IConfigurationRoot BuildConfigurationRoot()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsecrets.json", false, true);
            IConfigurationRoot root = builder.Build();
            return root;
        }

        static void Main(string[] args)
        {
            try
            {
                var container = CompositionRoot();
                Logger = container.Resolve<ILogger>();
                var databaseInitializer = container.Resolve<DatabaseInitializer>();
                databaseInitializer.InitializeDb();
                var app = container.Resolve<Application>();
                app.StartApplication();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Logger.Error($"Unhandled exception in Main : {e}");
            }
        }
    }
}
