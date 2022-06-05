using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Helpers.Time
{
    public class Time
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }

        public Time(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public Time(TimeWithSuffix timeWithSuffix)
        {
            Minutes = timeWithSuffix.Minutes;
            if (timeWithSuffix.Hours == 12 && timeWithSuffix.Suffix == "AM")
            {
                Hours = 0;
            }
            else if (timeWithSuffix.Hours == 12 && timeWithSuffix.Suffix == "PM")
            {
                Hours = 12;
            }
            else if (timeWithSuffix.Suffix == "PM")
            {
                Hours = timeWithSuffix.Hours + 12;
            }
            else
            {
                Hours = timeWithSuffix.Hours;
            }
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
            return s.ToString();
        }
    }
}
