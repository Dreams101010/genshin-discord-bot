using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ResultModels
{
    public struct TimeToResin
    {
        public DateTime TimeToResinUtc { get; set; }
        public TimeSpan TimeSpanToResin { get; set; }
    }
    public struct ResinInfoResultModel
    {
        public ResinInfoResultModel(int currentCount, 
            TimeSpan timeToFullResin, 
            Dictionary<int, TimeToResin> completionTimes)
        {
            CurrentCount = currentCount;
            TimeToFullResin = timeToFullResin;
            CompletionTimes = completionTimes;
        }

        public bool IsEmpty { get; init; } = false;
        public int CurrentCount { get; set; }
        public TimeSpan TimeToFullResin { get; set; }
        public Dictionary<int, TimeToResin> CompletionTimes { get; set; }
        public static ResinInfoResultModel Empty { get => new() { IsEmpty = true }; }

        public override bool Equals(object? obj)
        {
            return obj is ResinInfoResultModel model && Equals(model);
        }

        public bool Equals(ResinInfoResultModel other)
        {
            if (this.IsEmpty && other.IsEmpty)
            {
                return true; // both models are empty and considered equal
            }
            else
            {
                return IsEmpty == other.IsEmpty &&
                   CurrentCount == other.CurrentCount &&
                   TimeToFullResin.Equals(other.TimeToFullResin) &&
                   EqualityComparer<Dictionary<int, TimeToResin>>.Default.Equals(CompletionTimes, other.CompletionTimes);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsEmpty, CurrentCount, TimeToFullResin, CompletionTimes);
        }

        public static bool operator ==(ResinInfoResultModel left, ResinInfoResultModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ResinInfoResultModel left, ResinInfoResultModel right)
        {
            return !(left == right);
        }
    }
}
