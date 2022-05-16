using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.DomainModels
{
    public enum UserLocale
    {
        enGB, ruRU
    }
    public struct User : IEquatable<User>
    {
        public ulong DiscordId { get; set; }
        public UserLocale Locale { get; set; }

        public static User GetDefaultUser()
        {
            return new User
            {
                DiscordId = 0,
                Locale = UserLocale.enGB,
            };
        }

        public override bool Equals(object? obj)
        {
            return obj is User user && Equals(user);
        }

        public bool Equals(User other)
        {
            return DiscordId == other.DiscordId &&
                   Locale == other.Locale;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DiscordId, Locale);
        }

        public static bool operator ==(User left, User right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(User left, User right)
        {
            return !(left == right);
        }
    }
}
