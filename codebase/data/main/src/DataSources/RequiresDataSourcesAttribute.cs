﻿using System;
using System.Diagnostics.CodeAnalysis;
using Axle.Modularity;

namespace Axle.Data.DataSources
{
    /// <summary>
    /// Causes the annotated target module to become dependent on the data-sources module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class RequiresDataSourcesAttribute : RequiresAttribute
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RequiresDataSourcesAttribute"/> attribute.
        /// </summary>
        public RequiresDataSourcesAttribute() : base(typeof(DataSourceModule)) { }
    }
}