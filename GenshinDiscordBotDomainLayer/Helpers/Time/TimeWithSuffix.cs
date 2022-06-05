using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Helpers.Time
{
    public class TimeWithSuffix
    {
        public int Hours { get; }
        public int Minutes { get; }
        public string Suffix { get; }

        public TimeWithSuffix(int hours, int minutes, string suffix)
        {
            if (hours < 1 || hours > 12)
            {
                throw new ArgumentOutOfRangeException("Hours value is invalid for postfix time variant");
            }
            Hours = hours;
            Minutes = minutes;
            Suffix = suffix;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            if (Hours < 10)
            {
                s.Append('0');
            }
            s.Append(Hours);
            s.Append(':');
            if (Minutes < 10)
            {
                s.Append('0');
            }
            s.Append(Minutes);
            s.Append(' ');
            s.Append(Suffix);
            return s.ToString();
        }
    }
}
