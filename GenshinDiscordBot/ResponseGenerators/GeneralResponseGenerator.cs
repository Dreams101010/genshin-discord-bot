using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class GeneralResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        public GeneralResponseGenerator(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public string GetGeneralErrorMessage()
        {
            var nowUtc = DateTimeProvider.GetDateTime().ToUniversalTime();
            return string.Format(@"Something went wrong. 
Please contact the developer. 
The time of the event: {0}", nowUtc);
        }

        public string GetHelpMessage()
        {
            return @"!setLanguage - print available languages
!setLanguage <language> - set language
!printSettings - print settings
!setResin <resin count> - set current resin count
!getResin - get current resin info
!remindersOn - enable reminders for yourself
!remindersOff - disable reminder for yourself
!remindArtifacts - set a reminder about artifacts
!cancelRemindArtifacts - cancel the reminder about artifacts
!remindCheckIn - set a reminder about daily check-in
!cancelRemindCheckIn - cancel the reminder about daily check-in";
        }
    }
}
