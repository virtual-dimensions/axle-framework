﻿using System;
using System.Collections.Generic;
using System.Linq;

using Axle.Verification;


namespace Axle.Modularity
{
    public static class ModuleCatalogExtensions
    {
        #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
        internal static ModuleInfo[] GetModules(this IModuleCatalog moduleCatalog)
        {
            return GetModules(moduleCatalog, moduleCatalog.DiscoverModuleTypes());
        }
        #endif

        internal static ModuleInfo[] GetModules(this IModuleCatalog moduleCatalog, params Type[] types)
        {
            moduleCatalog.VerifyArgument(nameof(moduleCatalog)).IsNotNull();
            return ExtractModules(moduleCatalog, types, new Dictionary<Type, ModuleInfo>()).ToArray();
        }

        internal static ModuleInfo[] GetModules(this IModuleCatalog moduleCatalog, IEnumerable<Type> types)
        {
            moduleCatalog.VerifyArgument(nameof(moduleCatalog)).IsNotNull();
            return ExtractModules(moduleCatalog, types, new Dictionary<Type, ModuleInfo>()).ToArray();
        }

        private static void ExpandModules(IModuleCatalog catalog, Type moduleType, HashSet<Type> types)
        {
            if (!types.Add(moduleType))
            {
                return;
            }
            var modules = catalog.GetRequiredModules(moduleType);
            for (var i = 0; i < modules.Length; i++)
            {
                ExpandModules(catalog, modules[i], types);
            }
        }

        private static IEnumerable<ModuleInfo> ExtractModules(
                IModuleCatalog moduleCatalog, 
                IEnumerable<Type> types, 
                IDictionary<Type, ModuleInfo> knownModules)
        {
            var allModuleTypes = new HashSet<Type>();
            foreach (var v in types)
            {
                ExpandModules(moduleCatalog, v, allModuleTypes);
            }

            foreach (var moduleType in allModuleTypes)
            {
                var requiredModuleTypes = moduleCatalog.GetRequiredModules(moduleType).Except(new []{moduleType});
                var requiredModules = requiredModuleTypes
                        .Select(
                            t =>
                            {
                                if (!knownModules.TryGetValue(t, out var m))
                                {
                                    m = ExtractModules(moduleCatalog, new[] {t}, knownModules).Single(x => x.Type == t);
                                    knownModules.Add(t, m);
                                }
                                return m;
                            })
                        .ToArray();
                var utilizedModules = moduleCatalog.GetUtilizedModules(moduleType);
                var utilizedByModules = moduleCatalog.GetUtilizedByModules(moduleType);
                var configurationInfo = moduleCatalog.GetConfigurationInfo(moduleType);
                var module = new ModuleInfo(
                        moduleType,
                        moduleCatalog.GetInitMethod(moduleType),
                        moduleCatalog.GetDependencyInitializedMethods(moduleType),
                        moduleCatalog.GetDependencyTerminatedMethods(moduleType),
                        moduleCatalog.GetTerminateMethod(moduleType),
                        moduleCatalog.GetEntryPointMethod(moduleType),
                        utilizedModules,
                        utilizedByModules,
                        moduleCatalog.GetCommandLineTrigger(moduleType),
                        configurationInfo,
                        requiredModules);
                yield return module;
            }
        }
    }
}