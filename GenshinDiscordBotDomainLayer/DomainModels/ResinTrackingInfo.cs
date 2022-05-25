using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels
{
    public struct ResinTrackingInfo : IEquatable<ResinTrackingInfo>
    {
        public bool IsEmpty { get; init; } = false;
        public ulong UserDiscordId { get; set; }
        public DateTime StartTime { get; set; }
        public int StartCount { get; set; }

        public static ResinTrackingInfo Empty
        {
            get => new() { IsEmpty = true };
        }

        public ResinTrackingInfo(ulong userDiscordId, DateTime startTime, int startCount)
        {
            UserDiscordId = userDiscordId;
            StartTime = startTime;
            StartCount = startCount;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResinTrackingInfo info && Equals(info);
        }

        public bool Equals(ResinTrackingInfo other)
        {

            if (this.IsEmpty && other.IsEmpty)
            {
                return true; // both models are empty and considered equal
            }
            else
            {
                return IsEmpty == other.IsEmpty &&
                   UserDiscordId == other.UserDiscordId &&
                   StartTime == other.StartTime &&
                   StartCount == other.StartCount;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsEmpty, UserDiscordId, StartTime, StartCount);
        }

        public static bool operator ==(ResinTrackingInfo left, ResinTrackingInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ResinTrackingInfo left, ResinTrackingInfo right)
        {
            return !(left == right);
        }
    }
}
