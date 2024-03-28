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
using GenshinDiscordBotDomainLayer.Contexts;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotUI.Helpers;
using GenshinDiscordBotUI.CommandExecutors;
using GenshinDiscordBotUI.Models.SlashCommand;
using Autofac.Extras.DynamicProxy;
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using GenshinDiscordBotCrawler.Genshin;
using GenshinDiscordBotCrawler.Honkai;
using GenshinDiscordBotUI.SlashCommands;
using GenshinDiscordBotCrawler.HonkaiStarRail;

namespace GenshinDiscordBotUI
{
    // TODO: configure logging via file
    class Program
    {
        // Logger for logging unhandled exceptions
        static ILogger? Logger { get; set; } = null; 
        static IContainer CompositionRoot()
        {
            var localizationSource = ReadLocalizationSource();
            var slashCommandsSource = ReadSlashCommandSource();
            var assemblies = GetProjectAssemblies();
            var sqliteConnectionString = BuildConfigurationRoot().GetConnectionString("Sqlite");
            var builder = new ContainerBuilder();
            // Microsoft.Extensions.DependencyInjection support
            builder.Populate(new ServiceCollection());
            // Application
            builder.RegisterType<Application>();
            // Slash commands
            builder.Register((ctx) => slashCommandsSource).SingleInstance();
            builder.RegisterType<SlashCommandDispatcher>().SingleInstance();
            builder.RegisterType<UserSlashCommandHandler>().InstancePerLifetimeScope();
            builder.RegisterType<ResinSlashCommandHandler>().InstancePerLifetimeScope();
            builder.RegisterType<ReminderSlashCommandHandler>().InstancePerLifetimeScope();
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
            // Notifiers
            builder.RegisterType<DiscordNotifier>().As<INotifier>().SingleInstance();
            // Logger
            builder.Register<ILogger>(
                c => new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day, 
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Information()
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
            builder.RegisterType<ReminderArgumentValidator>().SingleInstance();
            builder.RegisterType<DateTimeArgumentValidator>().SingleInstance();
            // Contexts
            builder.RegisterType<DiscordContext>().InstancePerLifetimeScope();
            builder.RegisterType<UserContext>().InstancePerLifetimeScope();
            builder.RegisterType<RequestContext>().InstancePerLifetimeScope();
            // Helpers
            builder.RegisterType<UserHelper>().InstancePerLifetimeScope();
            builder.RegisterType<ReminderConversionHelper>().SingleInstance();
            // Providers
            builder.RegisterType<DateTimeNowProvider>()
                .As<IDateTimeProvider>().SingleInstance();
            // Parsers
            builder.RegisterType<GenshinWikiPromoTableParser>().AsSelf().SingleInstance();
            builder.RegisterType<HonkaiWikiPromoTableParser>().AsSelf().SingleInstance();
            builder.RegisterType<StarRailPromoTableParser>().AsSelf().SingleInstance();

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

        static SlashCommandSource ReadSlashCommandSource()
        {
            using var streamReader = new StreamReader("slashCommands.json");
            var json = streamReader.ReadToEnd();
            if (json == null)
            {
                throw new Exception("Couldn't read slash commands json from file");
            }
            SlashCommandListJsonModel? model = JsonSerializer.Deserialize<SlashCommandListJsonModel>(json);
            if (model == null)
            {
                throw new Exception("Couldn't deserialize json from file");
            }
            return model.ToSlashCommandSource();
        }

        [STAThread]
        static async Task Main(string[] args)
        {
            var container = CompositionRoot();
            Logger = container.Resolve<ILogger>();
            using var scope = container.BeginLifetimeScope();
            try
            {
                var databaseInitializer = scope.Resolve<DatabaseInitializer>();
                databaseInitializer.InitializeDb();
            }
            catch (Exception e)
            {
                Logger?.Error($"Unhandled exception in Main during initialization : {e}");
                throw;
            }
            try
            {
                var app = scope.Resolve<Application>();
                CancellationTokenSource cts = new();
                var botTask = app.StartApplication(cts.Token);
                await botTask;
            }
            catch (OperationCanceledException)
            {
                Logger?.Information("Application shutting down...");
            }
            catch (Exception e)
            {
                Logger?.Error($"Unhandled exception in Main : {e}");
            }
        }
    }
}