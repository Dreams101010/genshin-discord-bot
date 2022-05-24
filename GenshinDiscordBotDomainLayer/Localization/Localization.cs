﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Localization
{
    public class Localization
    {
        private LocalizationSource LocalizationSource { get; }
        public IReadOnlyDictionary<string, Dictionary<string, string>> English
        {
            get
            {
                return LocalizationSource.English;
            }
        }

        public IReadOnlyDictionary<string, Dictionary<string, string>> Russian
        {
            get
            {
                return LocalizationSource.Russian;
            }
        }

        public Localization(LocalizationSource localizationSource)
        {
            LocalizationSource = localizationSource ?? throw new ArgumentNullException(nameof(localizationSource));
        }
    }
}