using GenshinDiscordBotDomainLayer.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Contexts;
using Discord;
using Serilog;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public abstract class CommandExecutorBase
    {
        private IUserService UserService { get; set; }
        private ILogger Logger { get; }
        public RequestContext RequestContext { get; }
        private const ulong DM_GUILD_ID = 0;
        private const ulong DM_CHANNEL_ID = 0;

        protected CommandExecutorBase(IUserService userService, ILogger logger,
            RequestContext requestContext)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            RequestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public async void PopulateContextAsync(SocketCommandContext commandContext)
        {
            // populate from command context
            switch (commandContext.Channel.GetChannelType())
            {
                case ChannelType.DM:
                    {
                        RequestContext.DiscordContext.SetDiscordContextData(
                            DM_GUILD_ID, DM_CHANNEL_ID, commandContext.User.Id, 
                            commandContext.User.Username);
                        break;
                    }
                case ChannelType.Text:
                    {
                        RequestContext.DiscordContext.SetDiscordContextData(
                            commandContext.Guild.Id, commandContext.Channel.Id, 
                            commandContext.User.Id, commandContext.User.Username);
                        break;
                    }
                default:
                    {
                        Logger.Error("Invalid channel type enum state in PopulateContextAsync");
                        // throw exception so that it stops handling invalid request
                        throw new Exception("Invalid channel type enum state");
                    }
            }
            // create user if not exists and read it
            User user = await GetUserAsync(RequestContext.DiscordContext.UserId);
            // populate from user
            RequestContext.UserContext.User = user;
        }
        private async Task<User> GetUserAsync(ulong userId)
        {
            return await UserService.ReadUserAndCreateIfNotExistsAsync(userId);
        }
    }
}
