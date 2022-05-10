using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using GenshinDiscordBotDomainLayer.Parameters.Query;

namespace GenshinDiscordBotDomainLayer.DatabaseFacades
{
    public class UserDatabaseFacade
    {
        IUserRepository UserRepository { get; }
        public UserDatabaseFacade(IUserRepository userRepository)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User ReadUserAndCreateIfNotExists(ulong discordId)
        {
            // try get user
            User? nullableUser = UserRepository.GetUserByDiscordId(discordId);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return nullableUser.Value;
            }
            else
            {
                // if we don't have user create it
                CreateDefaultUserWithId(discordId);
                nullableUser = UserRepository.GetUserByDiscordId(discordId);
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    return nullableUser.Value;
                }
                else
                {
                    throw new Exception("Database interaction exception: could not read created user");
                }
            }
        }

        public void CreateUserIfNotExists(ulong discordId)
        {
            // try get user
            User? nullableUser = UserRepository.GetUserByDiscordId(discordId);
            bool hasUser = nullableUser.HasValue;
            if (hasUser)
            {
                return;
            }
            else
            {
                // if we don't have user create it
                CreateDefaultUserWithId(discordId);
                nullableUser = UserRepository.GetUserByDiscordId(discordId);
                hasUser = nullableUser.HasValue;
                if (hasUser)
                {
                    return;
                }
                else
                {
                    throw new Exception("Database interaction exception: could not read created user");
                }
            }
        }

        public void SetUserLocale(ulong discordId, UserLocale newLocale)
        {
            var user = ReadUserAndCreateIfNotExists(discordId);
            user.Locale = newLocale;
            AddOrUpdateUser(user);
        }

        private void CreateDefaultUserWithId(ulong discordId)
        {
            User newUser = User.GetDefaultUser();
            newUser.DiscordId = discordId;
            AddOrUpdateUser(newUser);
        }

        private void AddOrUpdateUser(User user)
        {
            var param = new AddOrUpdateUserCommandParam
            {
                User = user
            };
            UserRepository.InsertOrUpdateUser(user);
        }
    }
}
