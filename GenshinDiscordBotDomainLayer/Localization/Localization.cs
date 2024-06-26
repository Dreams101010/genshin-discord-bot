﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.Helpers;

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

        public string GetLocalizedString(string section, string name, UserLocale userLocale)
        {
            return userLocale switch
            {
                UserLocale.enGB => LocalizationSource.English[section][name],
                UserLocale.ruRU => LocalizationSource.Russian[section][name],
                _ => throw new NotImplementedException(),
            };
        }

        public string GetOptionalLocalizedString(string section, string name, UserLocale locale)
        {
            switch (locale)
            {
                case UserLocale.enGB:
                    {
                        if (!LocalizationSource.English.ContainsKey(section))
                        {
                            return string.Empty;
                        }
                        if (!LocalizationSource.English[section].ContainsKey(name))
                        {
                            return string.Empty;
                        }
                        return LocalizationSource.English[section][name];
                    }
                case UserLocale.ruRU:
                    {
                        if (!LocalizationSource.Russian.ContainsKey(section))
                        {
                            return string.Empty;
                        }
                        if (!LocalizationSource.Russian[section].ContainsKey(name))
                        {
                            return string.Empty;
                        }
                        return LocalizationSource.Russian[section][name];
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }
}
