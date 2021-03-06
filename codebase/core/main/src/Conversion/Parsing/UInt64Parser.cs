﻿using System;
using System.Globalization;


namespace Axle.Conversion.Parsing
{
    /// <summary>
    /// A class that can parse <see cref="string">string</see> representations of
    /// an <see cref="ulong">unsigned 64-bit integer</see> to a valid <see cref="ulong"/> value.
    /// </summary>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [Serializable]
    #endif
    public sealed class UInt64Parser : AbstractParser<UInt64>
    {
        /// <inheritdoc />
        protected override UInt64 DoParse(string value, IFormatProvider formatProvider)
        {
            return formatProvider != null ? UInt64.Parse(value, formatProvider) : UInt64.Parse(value);
        }

        /// <inheritdoc />
        public override bool TryParse(string value, IFormatProvider formatProvider, out UInt64 output)
        {
            return formatProvider != null
                ? UInt64.TryParse(value, NumberStyles.Any, formatProvider, out output)
                : UInt64.TryParse(value, out output);
        }
    }
}
