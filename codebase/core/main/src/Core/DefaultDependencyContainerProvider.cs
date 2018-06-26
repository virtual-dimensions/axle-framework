﻿#if NETSTANDARD1_5_OR_NEWER || !NETSTANDARD
using Axle.Core.DependencyInjection;


namespace Axle.Core
{
    public sealed class DefaultDependencyContainerProvider : IDependencyContainerProvider
    {
        public IContainer Create(IContainer parent) => new Container(parent);

        public IContainer Create() => new Container(null);
    }
}
#endif