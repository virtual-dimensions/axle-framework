﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Axle.Verification;


namespace Axle.Resources.Extraction
{
    public class ResourceExtractorChain : IResourceExtractorChain
    {
        private sealed class ChainCompositeResourceExtractor : IResourceExtractor
        {
            private readonly IResourceExtractor _a, _b;

            public ChainCompositeResourceExtractor(IResourceExtractor a, IResourceExtractor b)
            {
                _a = a;
                _b = b;
            }

            public ResourceInfo Extract(string name, CultureInfo culture)
            {
                if (_a is IResourceExtractorChain c)
                {
                    return c.Extract(name, culture, _b);
                }
                return _a.Extract(name, culture) ?? _b.Extract(name, culture);
            }
        }

        private IResourceExtractor _next;

        public ResourceExtractorChain(IEnumerable<IResourceExtractor> extractors) : this(
                extractors.VerifyArgument(nameof(extractors)).IsNotNull().Value.ToArray()) { }
        public ResourceExtractorChain(params IResourceExtractor[] extractors)
        {
            if (extractors.Length == 0)
            {
                _next = null;
            }
            else if (extractors.Length == 1)
            {
                _next = extractors[0];
            }
            else
            {
                var collapsed = extractors[extractors.Length - 1];
                for (var i = extractors.Length - 2; i >= 0; i--)
                {
                    collapsed = new ChainCompositeResourceExtractor(collapsed, extractors[i]);
                }
                _next = collapsed;
            }
        }

        public ResourceInfo Extract(string name, CultureInfo culture)
        {
            return _next == null ? null : Extract(name, culture, Next);
        }

        public virtual ResourceInfo Extract(string name, CultureInfo culture, IResourceExtractor nextInChain)
        {
            return nextInChain.Extract(name, culture);
        }

        protected IResourceExtractor Next => _next;
    }
}