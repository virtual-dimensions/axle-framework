﻿using System;
using Axle.Configuration.Text.Documents;
using Axle.Text.Documents.Properties;
using Axle.Verification;

namespace Axle.Configuration
{
    public sealed class PropertiesConfigSource : IConfigSource
    {
        private const string DefaultConfigFileName = "application{0}.properties";
        
        private readonly string _fileName;
        private readonly IConfigurationStreamProvider _configStreamProvider;

        public PropertiesConfigSource(IConfigurationStreamProvider configStreamProvider, string env)
        {
            Verifier.IsNotNull(Verifier.VerifyArgument(configStreamProvider, nameof(configStreamProvider)));
            _configStreamProvider = configStreamProvider;
            var envFormat = string.IsNullOrEmpty(env) ? string.Empty : $".{env}";
            _fileName = string.Format(DefaultConfigFileName, envFormat);
        }
        public PropertiesConfigSource(IConfigurationStreamProvider configStreamProvider) 
            : this(configStreamProvider, string.Empty) { }

        public IConfiguration LoadConfiguration()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var reader = new PropertiesDocumentReader(comparer);
            return new SafeConfigSource(
                    new StreamDocumentConfigSource(reader, () => _configStreamProvider.LoadConfiguration(_fileName)))
                .LoadConfiguration();
        }
    }
}