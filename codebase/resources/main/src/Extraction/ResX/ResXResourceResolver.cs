﻿#if !NETSTANDARD || NETSTANDARD2_0_OR_NEWER
using System;
using System.Globalization;

using Axle.Verification;


namespace Axle.Resources.Extraction.ResX
{
    public sealed class ResXResourceResolver
    {
        private readonly Type _resourceType;

        internal ResXResourceResolver(Type resourceType) 
        {
            _resourceType = resourceType.VerifyArgument(nameof(resourceType)).IsNotNull();
        }

        public object Resolve(Uri location, CultureInfo culture)
        {
            var resourceSet = new System.Resources.ResourceManager(_resourceType).GetResourceSet(culture, true, false);
            if (resourceSet == null)
            {
                return null;
            }
            using (resourceSet)
            {
                return resourceSet.GetObject(location.ToString().TrimStart('/'));
            }
        }
    }
}
#endif