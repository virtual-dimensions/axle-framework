﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Axle.Configuration;
#if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
using Axle.Configuration.Legacy;
#endif
using Axle.DependencyInjection;
using Axle.Modularity;

namespace Axle
{
    partial class Application
    {
        private sealed partial class Builder
        {
            private readonly object _syncRoot = new object();
            private readonly IModuleCatalog _moduleCatalog = new DefaultModuleCatalog();
            private readonly IList<Type> _moduleTypes = new List<Type>();
            private readonly IList<Action<IDependencyContainer>> _onContainerReadyHandlers = new List<Action<IDependencyContainer>>();

            private LayeredConfigManager _config = new LayeredConfigManager();
            private IApplicationHost _host = DefaultApplicationHost.Instance;

            private Builder Load(IEnumerable<Type> types)
            {
                foreach (var type in types)
                {
                    if (type != null)
                    {
                        _moduleTypes.Add(type);
                    }
                }
                return this;
            }

            private void PrintLogo()
            {
                foreach (var logoLine in _host.Logo)
                {
                    Console.WriteLine(logoLine);
                }
            }

            public Application Run(params string[] args)
            {
                PrintLogo();
                try 
                {
                    var rootContainer = _host.DependencyContainerFactory.CreateContainer();
                    rootContainer
                        .Export(new ApplicationContainerFactory(_host.DependencyContainerFactory, rootContainer))
                        .Export(_host.LoggingService)
                        .Export(_host);
                    
                    var generalConfig = Configure(new LayeredConfigManager(), this, string.Empty);
                    var envSpecificConfig = Configure(new LayeredConfigManager(), this, _host.EnvironmentName);
                    
                    var config = new LayeredConfigManager()
                        .Append(EnvironmentConfigSource.Instance)
                        .Append(new PreloadedConfigSource(_host.Configuration))
                        .Append(_config)
                        .Append(generalConfig)
                        .Append(envSpecificConfig)
                        .LoadConfiguration()
                        ;
                    var modulesConfigSection = config
                        .GetIncludeExcludeCollection<Type>("axle.application.modules");
                    
                    foreach (var moduleType in modulesConfigSection.IncludeElements)
                    {
                        _moduleTypes.Add(moduleType);
                    }
                    foreach (var moduleType in modulesConfigSection.ExcludeElements)
                    {
                        _moduleTypes.Remove(moduleType);
                    }
                    
                    foreach (var onContainerReadyHandler in _onContainerReadyHandlers)
                    {
                        onContainerReadyHandler.Invoke(rootContainer);
                    }
                    var app = Launch(
                        new ApplicationModuleCatalog(_moduleCatalog), _moduleTypes, _host, rootContainer, config, args);
                    app.Run();
                    return app;
                }
                catch (Exception)
                {
                    if (_host is IDisposable d)
                    {
                        d.Dispose();
                    }
                    throw;
                }
            }
        }
    }
}
