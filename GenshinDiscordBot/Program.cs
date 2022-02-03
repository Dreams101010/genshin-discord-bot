using System;
using System.Threading.Tasks;
using Autofac;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks;

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
        static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.Register(c => BuildConfigurationRoot()).InstancePerLifetimeScope();
            builder.RegisterType<Bot>();
            builder.RegisterType<DiscordSocketClient>().InstancePerLifetimeScope();
            builder.Register(c => new CommandService()).InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().InstancePerLifetimeScope();
            builder.Register<ILogger>(
                c => new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).SingleInstance();
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
            var container = CompositionRoot();
            var app = container.Resolve<Application>();
            app.StartApplication();
            Console.ReadLine();
        }
    }
}
