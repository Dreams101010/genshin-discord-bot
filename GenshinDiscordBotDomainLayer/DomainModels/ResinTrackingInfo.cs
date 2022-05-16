using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels
{
    public struct ResinTrackingInfo : IEquatable<ResinTrackingInfo>
    {
        public ulong UserDiscordId { get; set; }
        public DateTime StartTime { get; set; }
        public int StartCount { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ResinTrackingInfo info && Equals(info);
        }

        public bool Equals(ResinTrackingInfo other)
        {
            return UserDiscordId == other.UserDiscordId &&
                   StartTime == other.StartTime &&
                   StartCount == other.StartCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserDiscordId, StartTime, StartCount);
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
