using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces.Services
{
    public interface INotificationService
    {
        public void Start();
        public void Stop();
    }
}
