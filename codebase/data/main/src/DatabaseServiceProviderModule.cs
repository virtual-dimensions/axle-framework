﻿using Axle.Modularity;

namespace Axle.Data
{
    [Module]
    [Requires(typeof(DbServiceProviderRegistry))]
    public abstract class DatabaseServiceProviderModule
    {
        protected DatabaseServiceProviderModule(IDbServiceProvider provider)
        {
            Provider = provider;
        }

        internal IDbServiceProvider Provider { get; }
    }
}
