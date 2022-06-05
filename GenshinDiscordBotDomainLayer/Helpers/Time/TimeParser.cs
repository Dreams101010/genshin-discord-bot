using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pidgin;
using static Pidgin.Parser;

namespace GenshinDiscordBotDomainLayer.Helpers.Time
{
    public class TimeParser
    {
        static Parser<char, T> Tok<T>(Parser<char, T> p)
            => Try(p).Before(SkipWhitespaces);

        public static Time ParseTime(string time)
        {
            var timeParser = Try(Try(GetTimeWithPostfixParser().Map(x => new Time(x)))).Or(GetTimeParser());
            var result = timeParser.Parse(time);
            if (result.Success)
            {
                return result.Value;
            }
            else
            {
                throw new ArgumentException("Invalid time format or value");
            }
        }

        public static bool TryParseTime(string time, out Time outTime)
        {
            try
            {
                outTime = ParseTime(time);
                return true;
            }
            catch
            {
                outTime = null;
                return false;
            }
        }

        private static Parser<char, Time> GetTimeParser()
        {
            var combinedParser =
                from hours in Try(GetHoursParser())
                from colon in Try(Char(':'))
                from minutes in Try(GetMinutesParser())
                select new Time(hours, minutes);
            return combinedParser;
        }

        private static Parser<char, TimeWithSuffix> GetTimeWithPostfixParser()
        {
            var combinedParser =
                from hours in Try(GetHoursParser())
                from colon in Try(Char(':'))
                from minutes in Try(GetMinutesParser())
                from postfix in Try(GetSuffixParser())
                select new TimeWithSuffix(hours, minutes, postfix);
            return combinedParser;
        }

        private static Parser<char, int> GetHoursParser()
        {
            var firstDigit0to1 = Try(Digit.Where(c => c <= '1'));
            var firstDigit2 = Try(Digit.Where(c => c == '2'));
            var secondDigitIf0to1 = Try(Digit);
            var secondDigitIf2 = Try(Digit.Where(c => c <= '3'));
            var onlySecondDigit = Try(Digit);

            var hours0to1 = Try(
                from first in firstDigit0to1
                from second in secondDigitIf0to1
                select new string(new[] { first, second }));
            var hours2 = Try(
                from first in firstDigit2
                from second in secondDigitIf2
                select new string(new[] { first, second }));
            var hoursWithSecondDigit = Try(
                from second in onlySecondDigit
                select new string(new[] { second }));
            var hoursParser = Tok(hours0to1.Or(hours2).Or(hoursWithSecondDigit))
                .Map((x) => Convert.ToInt32(x));
            return hoursParser;
        }

        private static Parser<char, int> GetMinutesParser()
        {
            var firstDigitParser = Try(Digit.Where((c) => c <= '5'));
            var secondDigitIf0to1Parser = Try(Digit);
            var minutesParser = Tok(
                from firstDigit in firstDigitParser
                from secondDigit in secondDigitIf0to1Parser
                select new string(new[] { firstDigit, secondDigit }))
                .Map((x) => Convert.ToInt32(x));
            return minutesParser;
        }

        private static Parser<char, string> GetSuffixParser()
        {
            var firstLetterParser = Try(Letter.Where((c) => c == 'A' || c == 'P'));
            var secondLetterParser = Try(Letter.Where(c => c == 'M'));
            var parser = Tok(
                from firstLetter in firstLetterParser
                from secondLetter in secondLetterParser
                select new string(new[] { firstLetter, secondLetter }));
            return parser;
        }
    }
}
