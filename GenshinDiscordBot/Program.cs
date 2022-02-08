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
using GenshinDiscordBotSQLiteDataAccessLayer.Commands;
using GenshinDiscordBotSQLiteDataAccessLayer.Queries;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DatabaseFacades;
using GenshinDiscordBotDomainLayer.CommandFacades;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.Providers;
using GenshinDiscordBotDomainLayer.ErrorHandlers;
using GenshinDiscordBotDomainLayer.DataProviders;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.Helpers;

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
            // Microsoft.Extensions.DependencyInjection support
            builder.Populate(new ServiceCollection());
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
            builder.RegisterType<SQLiteConnectionProvider>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseInitializer>().SingleInstance();
            // User Commands
            builder.RegisterType<AddOrUpdateUserCommand>()
                .As(typeof(ICommand<AddOrUpdateUserCommandParam, bool>))
                .InstancePerLifetimeScope();
            // User Queries
            builder.RegisterType<GetUserByDiscordIdQuery>()
                .As(typeof(IQuery<GetUserByDiscordIdQueryParam, User?>))
                .InstancePerLifetimeScope();
            // Resin Commands
            builder.RegisterType<AddOrUpdateResinInfoCommand>()
                .As(typeof(ICommand<AddOrUpdateResinInfoCommandParam, bool>))
                .InstancePerLifetimeScope();
            // Resin Queries
            builder.RegisterType<GetResinInfoByDiscordIdQuery>()
                .As(typeof(IQuery<GetResinInfoByDiscordIdQueryParam, ResinTrackingInfo?>))
                .InstancePerLifetimeScope();
            // Repositories
            builder.RegisterType<UserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ResinTrackingInfoRepository>().InstancePerLifetimeScope();
            // Command Facades
            builder.RegisterType<UserFacade>().InstancePerLifetimeScope();
            builder.RegisterType<ResinFacade>().InstancePerLifetimeScope();
            // Database Facades
            builder.RegisterType<UserDatabaseFacade>().InstancePerLifetimeScope();
            builder.RegisterType<ResinDatabaseFacade>().InstancePerLifetimeScope();
            // Business Logic
            builder.RegisterType<ResinBusinessLogic>().SingleInstance();
            // Validation Logic
            builder.RegisterType<ResinCommandArgumentValidator>().SingleInstance();
            // Helpers
            builder.RegisterType<UserHelper>().InstancePerLifetimeScope();
            // Error handlers
            builder.RegisterType<FacadeErrorHandler>().SingleInstance();
            // Providers
            builder.RegisterType<DateTimeNowProvider>()
                .As<IDateTimeProvider>().SingleInstance();
            // Data Providers
            builder.RegisterType<ResinDataProvider>().SingleInstance();
            // Response generators
            builder.RegisterType<UserResponseGenerator>().SingleInstance();
            builder.RegisterType<ResinResponseGenerator>().SingleInstance();
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

        static void Main(string[] args)
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
