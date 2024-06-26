﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Helpers;

namespace GenshinDiscordBotSQLiteDataAccessLayer.DataModels
{
    public struct UserDataModel
    {
        public ulong DiscordId { get; set; }
        public string Locale { get; set; }
        public bool RemindersOptIn { get; set; } = true;

        public UserDataModel(User userFromDomain)
        {
            DiscordId = userFromDomain.DiscordId;
            Locale = EnumConversionHelper.UserLocaleToString(userFromDomain.Locale);
            RemindersOptIn = userFromDomain.RemindersOptIn;
        }

        public User ToUserDomain()
        {
            return new User
            {
                DiscordId = this.DiscordId,
                Locale = EnumConversionHelper.UserLocaleFromString(Locale),
                RemindersOptIn = this.RemindersOptIn
            };
        }
    }
}
