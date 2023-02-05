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

namespace GenshinDiscordBotUI.CommandExecutors
{
    public abstract class CommandExecutorBase
    {
        private IUserService UserService { get; set; }
        public RequestContext RequestContext { get; }
        private const ulong DM_GUILD_ID = 0;
        private const ulong DM_CHANNEL_ID = 0;

        protected CommandExecutorBase(IUserService userService, RequestContext requestContext)
        {
            UserService = userService;
            RequestContext = requestContext;
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
                        throw new Exception("Invalid channel type enum state");
                        // TODO: gracefully return error here
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
