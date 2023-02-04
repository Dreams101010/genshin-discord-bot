using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.BusinessLogic;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ResultModels;

namespace GenshinDiscordBotDomainLayer.Helpers
{
    public class ReminderConversionHelper
    {
        public DateTimeBusinessLogic DateTimeBusinessLogic { get; }
        public ReminderConversionHelper(DateTimeBusinessLogic dateTimeBusinessLogic)
        {
            DateTimeBusinessLogic = dateTimeBusinessLogic ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
        }

        public ReminderResultModel GetReminderResultModel(Reminder reminder)
        {
            ReminderResultModel resultModel = new ReminderResultModel()
            {
                SetupTime = DateTimeBusinessLogic.GetLocalTimeFromUnixSeconds(
                    reminder.ReminderTime - reminder.Interval),
                ReminderTime = DateTimeBusinessLogic.GetLocalTimeFromUnixSeconds(reminder.ReminderTime),
                CategoryName = reminder.CategoryName,
                Message = reminder.Message,
                Id = reminder.Id,
                Interval = TimeSpan.FromSeconds(reminder.Interval),
                IsRecurrent = reminder.RecurrentFlag

            };
            return resultModel;
        }

        public List<ReminderResultModel> GetReminderResultModelList(List<Reminder> reminders)
        {
            return reminders.Select(x => GetReminderResultModel(x)).ToList();
        }
    }
}
