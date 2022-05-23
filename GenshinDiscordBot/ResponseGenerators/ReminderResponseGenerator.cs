using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class ReminderResponseGenerator
    {
        public string GetArtifactReminderSetupSuccessMessage()
        {
            return "Okay! I will remind you about artifacts in 24 hours.";
        }

        public string GetArtifactReminderCancelSuccessMessage()
        {
            return "The artifact reminder has been cancelled.";
        }

        public string GetArtifactReminderCancelNotFoundMessage()
        {
            return "No artifact reminders were found for you.";
        }
    }
}
