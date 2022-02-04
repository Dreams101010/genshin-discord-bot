using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;

namespace GenshinDiscordBotDomainLayer.Parameters.Command
{
    public class AddOrUpdateUserCommandParam
    {
        public User User { get; set; }
    }
}
