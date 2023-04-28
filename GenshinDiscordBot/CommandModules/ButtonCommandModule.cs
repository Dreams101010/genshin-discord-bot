using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.CommandModules
{
    public class ButtonCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("createGenshinPromocodesRoleButtons")]
        public async Task CreateGenshinPromocodesRoleButtons()
        {
            var builder = new ComponentBuilder()
                .WithButton("Give Genshin Promocode role", "genshin-promocode-role-give")
                .WithButton("Remove Genshin Promocode role", "genshin-promocode-role-remove");

            await ReplyAsync("Use these to give or remove the Genshin Promocode role from yourself.", components: builder.Build());
        }

        [Command("createHonkaiPromocodesRoleButtons")]
        public async Task CreateHonkaiPromocodesRoleButtons()
        {
            var builder = new ComponentBuilder()
                .WithButton("Give Honkai Impact 3rd Promocode role", "honkai-promocode-role-give")
                .WithButton("Remove Honkai Impact 3rd Promocode role", "honkai-promocode-role-remove");

            await ReplyAsync("Use these to give or remove the Honkai Promocode role from yourself.", components: builder.Build());
        }

        [Command("createHonkaiStarRailRoleButtons")]
        public async Task CreateHonkaiStarRailPromocodesRoleButtons()
        {
            var builder = new ComponentBuilder()
                .WithButton("Give Honkai Star Rail Promocode role", "honkai-star-rail-promocode-role-give")
                .WithButton("Remove Honkai Star Rail Promocode role", "honkai-star-rail-promocode-role-remove");

            await ReplyAsync("Use these to give or remove the Honkai Star Rail Promocode role from yourself.", components: builder.Build());
        }
    }
}
