﻿#if NETSTANDARD2_0_OR_NEWER || NET461_OR_NEWER
using System.Collections.Generic;

namespace Axle.Configuration.Microsoft.Adapters
{
    using IMSConfiguration = global::Microsoft.Extensions.Configuration.IConfiguration;
    using IMSConfigurationProvider = global::Microsoft.Extensions.Configuration.IConfigurationProvider;
    using MSConfigurationProvider = global::Microsoft.Extensions.Configuration.ConfigurationProvider;
    using MSConfigurationPath = global::Microsoft.Extensions.Configuration.ConfigurationPath;

    /// <summary>
    /// An implementation of the <see cref="IMSConfigurationProvider"/> which exposes an instance of
    /// <see cref="IConfigSection"/> to be used as configuration source where <see cref="IMSConfiguration"/> objects
    /// are required.
    /// </summary>
    internal sealed class AxleConfigurationProvider : MSConfigurationProvider
    {
        private static void Parse(string key, IConfigSetting setting, IDictionary<string, string> data)
        {
            if (setting == null)
            {
                return;
            }
            var isValidKey = !string.IsNullOrEmpty(key);
            if (isValidKey && !string.IsNullOrEmpty(setting.Value))
            {
                data[key] = setting.Value;
            }
            if (setting is IConfigSection section)
            {
                foreach (var sectionKey in section.Keys)
                {
                    var parsedKey = isValidKey ? MSConfigurationPath.Combine(key, sectionKey) : sectionKey;
                    Parse(parsedKey, section[sectionKey], data);
                }
            }
        }

        public AxleConfigurationProvider(IConfigSection rootConfiguration)
        {
            Parse(null, rootConfiguration, Data);
        }
    }
}
#endif