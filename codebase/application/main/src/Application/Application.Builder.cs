﻿using System;
using System.Collections.Generic;
using Axle.Configuration;
using Axle.DependencyInjection;
using Axle.Logging;
using Axle.Modularity;

namespace Axle.Application
{
    partial class Application
    {
        private sealed partial class Builder
        {
            private readonly object _syncRoot = new object();
            private readonly IModuleCatalog _moduleCatalog = new DefaultModuleCatalog();
            private readonly ICollection<Type> _moduleTypes = new HashSet<Type>();
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

            private void PrintLogo(IConfiguration config)
            {
                foreach (var logoLine in config.GetSections("axle.application.asciilogo"))
                {
                    Console.WriteLine(logoLine.Value);
                }
            }

            public Application Run(params string[] args)
            {
                try 
                {
                    var loggingService = new MutableLoggingService(_host.LoggingService ?? new AxleLoggingService());
                    
                    if (_host is ISetLoggingService sls)
                    {
                        sls.LoggingService = loggingService;
                    }
                    
                    var rootContainer = _host.DependencyContainerFactory.CreateContainer();
                    rootContainer
                        .Export(new ApplicationContainerFactory(_host.DependencyContainerFactory, rootContainer))
                        // TODO consider not exporting this service
                        .Export(loggingService)
                        .Export(_host);
                    var configMgr = new LayeredConfigManager().Append(EnvironmentConfigSource.Instance);
                    var hostCfg = _host.HostConfiguration;
                    if (hostCfg != null)
                    {
                        configMgr = configMgr.Append(hostCfg);
                    }
                    
                    configMgr = configMgr.Append(_config);
                    
                    var appCfg = _host.AppConfiguration;
                    if (appCfg != null)
                    {
                        configMgr = configMgr.Append(appCfg);
                    }
                    var config = configMgr.LoadConfiguration();
                    
                    PrintLogo(config);
                    
                    var modulesConfigSection = config.GetIncludeExcludeCollection<Type>("axle.application.modules");
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
                        new ApplicationModuleCatalog(_moduleCatalog), _moduleTypes, _host, loggingService, rootContainer, config, args);
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
