﻿using System;
using System.Globalization;


namespace Axle.Conversion.Parsing
{
    /// <summary>
    /// A class that can parse <see cref="string">string</see> representations of
    /// an <see cref="ushort">unsigned 16-bit integer</see> to a valid <see cref="ushort"/> value.
    /// </summary>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [System.Serializable]
    #endif
    public sealed class UInt16Parser : AbstractParser<ushort>
    {
        /// <inheritdoc />
        protected override UInt16 DoParse(string value, IFormatProvider formatProvider)
        {
            return formatProvider != null ? UInt16.Parse(value, formatProvider) : UInt16.Parse(value);
        }

        /// <inheritdoc />
        public override bool TryParse(string value, IFormatProvider formatProvider, out UInt16 output)
        {
            return formatProvider != null
                ? UInt16.TryParse(value, NumberStyles.Any, formatProvider, out output)
                : UInt16.TryParse(value, out output);
        }
    }
}
