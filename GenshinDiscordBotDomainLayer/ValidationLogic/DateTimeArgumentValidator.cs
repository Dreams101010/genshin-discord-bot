using GenshinDiscordBotDomainLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ValidationLogic
{
    public class DateTimeArgumentValidator
    {
        private IDateTimeProvider DateTimeProvider { get; set; }
        public DateTimeArgumentValidator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public bool IsInFuture(DateTime dateTime)
        {
            var now = DateTimeProvider.GetDateTime();
            if (dateTime < now)
            {
                return false;
            }
            return true;
        }
    }
}
