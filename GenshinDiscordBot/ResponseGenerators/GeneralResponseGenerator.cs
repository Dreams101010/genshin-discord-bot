﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.Interfaces;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Localization;

namespace GenshinDiscordBotUI.ResponseGenerators
{
    public class GeneralResponseGenerator
    {
        private IDateTimeProvider DateTimeProvider { get; }
        private Localization Localization { get; }
        public GeneralResponseGenerator(IDateTimeProvider dateTimeProvider,
            Localization localization)
        {
            DateTimeProvider = dateTimeProvider
                ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            Localization = localization ?? throw new ArgumentNullException(nameof(localization));

        }

        public string GetGeneralErrorMessage()
        {
            
            var nowUtc = DateTimeProvider.GetDateTime().ToUniversalTime();
            return string.Format("Something went wrong.\nPlease contact the developer.\nThe time of the event: {0}", nowUtc);
        }

        public string GetHelpMessage(UserLocale locale)
        {
            var format = Localization.GetLocalizedString("General", "HelpMessage", locale);
            return format;
        }
    }
}
