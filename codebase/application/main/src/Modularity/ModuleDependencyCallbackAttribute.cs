﻿namespace Axle.Modularity
{
    public abstract class ModuleDependencyCallbackAttribute : ModuleCallbackAttribute
    {
        public int Priority { get; set; } = 0;
    }
}