using System;
using System.IO;
using System.Text.Json;
using System.Threading;
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
using GenshinDiscordBotDomainLayer.Localization;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using GenshinDiscordBotDomainLayer.Providers;
using GenshinDiscordBotDomainLayer.ErrorHandlers;
using GenshinDiscordBotDomainLayer.DataProviders;
using GenshinDiscordBotDomainLayer.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.CommandExecutors;
using Autofac.Extras.DynamicProxy;
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace GenshinDiscordBotUI
{
    class Application
    {
        public Bot Bot { get; }

        public Application(Bot bot)
        {
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public async Task StartApplication(CancellationToken token)
        {
            await Bot.StartBot(token);
        }
    }
    class Program
    {
        // Logger for logging unhandled exceptions
        static ILogger? Logger { get; set; } = null; 
        static IContainer CompositionRoot()
        {
            var localizationSource = ReadLocalizationSource();
            var assemblies = GetProjectAssemblies();
            var sqliteConnectionString = BuildConfigurationRoot().GetConnectionString("Sqlite");
            var builder = new ContainerBuilder();
            // Microsoft.Extensions.DependencyInjection support
            builder.Populate(new ServiceCollection());
            // Application
            builder.RegisterType<Application>();
            // Localization
            builder.Register((ctx) => localizationSource).SingleInstance();
            builder.RegisterType<Localization>().SingleInstance();
            // Configuration
            builder.Register(c => BuildConfigurationRoot()).InstancePerLifetimeScope();
            // Discord.NET
            builder.RegisterType<Bot>();
            builder.RegisterType<DiscordSocketClient>().SingleInstance();
            builder.Register(c => new CommandService()).InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().InstancePerLifetimeScope();
            // Bot helpers
            builder.RegisterType<BotMessageSender>().As<IBotMessageSender>().SingleInstance();
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
            // Business Logic
            builder.RegisterType<ResinBusinessLogic>().SingleInstance();
            builder.RegisterType<ReminderMessageBusinessLogic>().SingleInstance();
            builder.RegisterType<DateTimeBusinessLogic>().SingleInstance();
            // Validation Logic
            builder.RegisterType<ResinCommandArgumentValidator>().SingleInstance();
            // Helpers
            builder.RegisterType<UserHelper>().InstancePerLifetimeScope();
            builder.RegisterType<ReminderConversionHelper>().SingleInstance();
            // Providers
            builder.RegisterType<DateTimeNowProvider>()
                .As<IDateTimeProvider>().SingleInstance();

            foreach (var assembly in assemblies)
            {
                // Interceptors
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("Interceptor"))
                    .SingleInstance();
                // Repositories
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
                // Response generators
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("ResponseGenerator"))
                    .SingleInstance();
                // Command executors
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("CommandExecutor"))
                    .InstancePerLifetimeScope();
                // Data Providers
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("DataProvider"))
                    .SingleInstance();
                // Error handlers
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("ErrorHandler"))
                    .SingleInstance();
                // Services
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    .InterceptedBy(typeof(ErrorHandlingInterceptor))
                    .EnableInterfaceInterceptors();
                // Database Interaction Handlers
                builder.RegisterAssemblyTypes(assembly)
                    .Where((t) => t.Name.EndsWith("SqliteDatabaseInteractionHandler"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    .InterceptedBy(typeof(LoggingInterceptor))
                    .InterceptedBy(typeof(ErrorHandlingInterceptor))
                    .InterceptedBy(typeof(RetryInterceptor))
                    .EnableInterfaceInterceptors();
            }
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

        static IEnumerable<Assembly> GetProjectAssemblies()
        {
            Assembly thisAssembly = typeof(Program).Assembly;
            var assemblyNames = thisAssembly.GetReferencedAssemblies()
                .Where((a) => a.Name?.StartsWith("GenshinDiscordBot") ?? false);
            List<Assembly> result = new List<Assembly>();
            foreach (var i in assemblyNames)
            {
                result.Add(Assembly.Load(i));
            }
            result.Add(thisAssembly);
            return result;
        }

        static LocalizationSource ReadLocalizationSource()
        {
            using var streamReader = new StreamReader("localization.json");
            var json = streamReader.ReadToEnd();
            if (json == null)
            {
                throw new Exception("Couldn't read localization json from file");
            }
            LocalizationSource? localizationSource = JsonSerializer.Deserialize<LocalizationSource>(json);
            if (localizationSource == null)
            {
                throw new Exception("Couldn't deserialize json from file");
            }
            return localizationSource;
        }

        static async Task Main(string[] args)
        {
            try
            {
                var container = CompositionRoot();
                Logger = container.Resolve<ILogger>();
                using var scope = container.BeginLifetimeScope();
                var databaseInitializer = scope.Resolve<DatabaseInitializer>();
                databaseInitializer.InitializeDb();
                var app = scope.Resolve<Application>();
                CancellationTokenSource cancellationTokenSource = new();
                CancellationToken token = cancellationTokenSource.Token;
                Logger?.Information("Application starting...");
                var appTask = app.StartApplication(token);
                Console.WriteLine("Bot will be online shortly. Press ENTER to shut the application down.");
                Console.ReadLine();
                Logger?.Information("Application shutting down...");
                cancellationTokenSource.Cancel();
                await appTask; // await app to finish shutting down properly
                Logger?.Information("Application has shut down.");
            }
            catch (OperationCanceledException)
            {
                Logger?.Information("Application shutting down...");
            }
            catch (Exception)
            {
                Logger?.Error("Unhandled exception in Main : {e}");
            }
        }
    }
}