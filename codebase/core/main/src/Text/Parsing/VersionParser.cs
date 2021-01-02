﻿using System;

namespace Axle.Text.Parsing
{
    /// <summary>
    /// A class that can parse <see cref="string">string</see> representations of
    /// a <see cref="Version">version number</see> to a valid <see cref="Version"/> value.
    /// </summary>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK || UNITY_2018_1_OR_NEWER
    [Serializable]
    #endif
    public sealed class VersionParser : AbstractParser<Version>
    {
        /// <inheritdoc />
        protected override Version DoParse(CharSequence value, IFormatProvider formatProvider)
        {
            // TODO: Specify version format provider

            return new Version(value.ToString());
        }
    }
}
