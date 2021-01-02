﻿#if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK || UNITY_2018_1_OR_NEWER
using System;
using System.Globalization;

namespace Axle.Text.Parsing
{
    /// <summary>
    /// A class that can parse <see cref="string">string</see> representations of
    /// a <see cref="Version">version number</see> to a valid <see cref="Version"/> value.
    /// </summary>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK || UNITY_2018_1_OR_NEWER
    [Serializable]
    #endif
    public sealed class CultureInfoParser : AbstractParser<CultureInfo>
    {
        /// <inheritdoc />
        protected override CultureInfo DoParse(CharSequence value, IFormatProvider formatProvider)
        {
            // see https://stackoverflow.com/questions/986754/when-to-use-cultureinfo-getcultureinfostring-or-cultureinfo-createspecificcult
            // for info why not using `CultureInfo.CreateSpecificCulture`
            return CultureInfo.GetCultureInfo(value.ToString());
        }
    }
}
#endif