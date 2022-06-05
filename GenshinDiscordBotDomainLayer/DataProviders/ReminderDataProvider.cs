using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DataProviders
{
    public class ReminderDataProvider
    {
        public TimeSpan MinTimeForSereniteaPotPlantHarvest { get; init; } = TimeSpan.Zero;
        public TimeSpan MaxTimeForSereniteaPotPlantHarvest { get; init; } = new TimeSpan(2, 22, 0, 0);
    }
}
