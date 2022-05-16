using System;
using System.Threading.Tasks;
using Autofac;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using GenshinDiscordBotSQLiteDataAccessLayer;
using GenshinDiscordBotSQLiteDataAccessLayer.Repositories;
using GenshinDiscordBotSQLiteDataAccessLayer.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Interceptors;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Interfaces.DatabaseInteractionHandlers;
using GenshinDiscordBotDomainLayer.Services;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.Providers;
using GenshinDiscordBotDomainLayer.ErrorHandlers;
using GenshinDiscordBotDomainLayer.DataProviders;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.CommandExecutors;
using Autofac.Extras.DynamicProxy;
using Microsoft.Data.Sqlite;

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
        static ILogger? Logger { get; set; } = null; 
        static IContainer CompositionRoot()
        {
            var sqliteConnectionString = BuildConfigurationRoot().GetConnectionString("Sqlite");
            Console.WriteLine(sqliteConnectionString);
            var builder = new ContainerBuilder();
            // Microsoft.Extensions.DependencyInjection support
            builder.Populate(new ServiceCollection());
            // Application
            builder.RegisterType<Application>();
            // Configuration
            builder.Register(c => BuildConfigurationRoot()).InstancePerLifetimeScope();
            // Discord.NET
            builder.RegisterType<Bot>();
            builder.RegisterType<DiscordSocketClient>().SingleInstance();
            builder.Register(c => new CommandService()).InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().InstancePerLifetimeScope();
            // Logger
            builder.Register<ILogger>(
                c => new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).SingleInstance();
            // Database connection
            builder.Register((c) => new SqliteConnection(sqliteConnectionString)).AsSelf().InstancePerLifetimeScope();
            // DAL
            builder.RegisterType<DatabaseInitializer>().SingleInstance();
            // Interceptors
            builder.RegisterType<RetryInterceptor>().AsSelf().SingleInstance();
            builder.RegisterType<LoggingInterceptor>().AsSelf().SingleInstance();
            builder.RegisterType<ErrorHandlingInterceptor>().AsSelf().SingleInstance();
            // Repositories
            builder.RegisterType<UserRepository>().As<IUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ResinTrackingInfoRepository>().As<IResinTrackingInfoRepository>()
                .InstancePerLifetimeScope();
            // Services
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(ErrorHandlingInterceptor))
                .EnableInterfaceInterceptors();
            builder.RegisterType<ResinService>()
                .As<IResinService>()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(ErrorHandlingInterceptor))
                .EnableInterfaceInterceptors();
            // Database Interaction Handlers
            builder.RegisterType<UserSqliteDatabaseInteractionHandler>()
                .As<IUserDatabaseInteractionHandler>()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(LoggingInterceptor))
                .InterceptedBy(typeof(ErrorHandlingInterceptor))
                .InterceptedBy(typeof(RetryInterceptor))
                .EnableInterfaceInterceptors();
            builder.RegisterType<ResinSqliteDatabaseInteractionHandler>()
                .As<IResinDatabaseInteractionHandler>()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(LoggingInterceptor))
                .InterceptedBy(typeof(ErrorHandlingInterceptor))
                .InterceptedBy(typeof(RetryInterceptor))
                .EnableInterfaceInterceptors();
            // Business Logic
            builder.RegisterType<ResinBusinessLogic>().SingleInstance();
            // Validation Logic
            builder.RegisterType<ResinCommandArgumentValidator>().SingleInstance();
            // Helpers
            builder.RegisterType<UserHelper>().InstancePerLifetimeScope();
            // Error handlers
            builder.RegisterType<LoggingErrorHandler>().AsSelf().SingleInstance();
            // Providers
            builder.RegisterType<DateTimeNowProvider>()
                .As<IDateTimeProvider>().SingleInstance();
            // Data Providers
            builder.RegisterType<ResinDataProvider>().SingleInstance();
            // Response generators
            builder.RegisterType<GeneralResponseGenerator>().SingleInstance();
            builder.RegisterType<UserResponseGenerator>().SingleInstance();
            builder.RegisterType<ResinResponseGenerator>().SingleInstance();
            // Command executors
            builder.RegisterType<UserCommandExecutor>().InstancePerLifetimeScope();
            builder.RegisterType<ResinCommandExecutor>().InstancePerLifetimeScope();
            return builder.Build();
        }

        static IConfigurationRoot BuildConfigurationRoot()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsecrets.json", false, true)
                .AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            return root;
        }

        static async Task Main(string[] args)
        {
            try
            {
                var container = CompositionRoot();
                Logger = container.Resolve<ILogger>();
                using (var scope = container.BeginLifetimeScope())
                {
                    var databaseInitializer = scope.Resolve<DatabaseInitializer>();
                    databaseInitializer.InitializeDb();
                }
                var app = container.Resolve<Application>();
                await app.StartApplication();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Logger?.Error($"Unhandled exception in Main : {e}");
            }
        }
    }
}
