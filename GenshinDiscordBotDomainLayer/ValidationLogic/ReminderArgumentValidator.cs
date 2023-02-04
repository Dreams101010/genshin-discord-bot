using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DataProviders;

namespace GenshinDiscordBotDomainLayer.ValidationLogic
{
    public class ReminderArgumentValidator
    {
        public ReminderDataProvider ReminderDataProvider { get; }
        public ReminderArgumentValidator(ReminderDataProvider reminderDataProvider)
        {
            ReminderDataProvider = reminderDataProvider ?? throw new ArgumentNullException(nameof(reminderDataProvider));
        }

        public bool UpdateOrCreateSereniteaPotPlantHarvestReminderAsync_TimeValid(int days, int hours)
        {
            TimeSpan timeToCheck = new TimeSpan(days, hours, 0, 0);
            if (timeToCheck <= ReminderDataProvider.MinTimeForSereniteaPotPlantHarvest 
                || timeToCheck > ReminderDataProvider.MaxTimeForSereniteaPotPlantHarvest)
            {
                return false;
            }
            return true;
        }

        public void UpdateOrCreateSereniteaPotPlantHarvestReminderAsync_Validate(int days, int hours)
        {
            var validationResult = UpdateOrCreateSereniteaPotPlantHarvestReminderAsync_TimeValid(days, hours);
            if (!validationResult)
            {
                throw new ArgumentException("Invalid value of a time interval");
            }
        }

        public bool UpdateOrCreateParametricTransformerReminderAsync_TimeValid(int days, int hours)
        {
            TimeSpan timeToCheck = new TimeSpan(days, hours, 0, 0);
            if (timeToCheck <= ReminderDataProvider.MinTimeForParametricTransformer
                || timeToCheck > ReminderDataProvider.MaxTimeForParametricTransformer)
            {
                return false;
            }
            return true;
        }

        public void UpdateOrCreateParametricTransformerReminderAsync_Validate(int days, int hours)
        {
            var validationResult = UpdateOrCreateParametricTransformerReminderAsync_TimeValid(days, hours);
            if (!validationResult)
            {
                throw new ArgumentException("Invalid value of a time interval");
            }
        }
    }
}
