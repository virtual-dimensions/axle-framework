﻿using System;
using System.Text;

using Axle.Verification;


namespace Axle.Extensions.Globalization.TextInfo
{
    using TextInfo = System.Globalization.TextInfo;

    /// <summary>
    /// A static class providing extension methods for <see cref="System.Globalization.TextInfo"/> instances.
    /// </summary>
    public static class TextInfoExtensions
    {
        /// <summary>
        /// Gets the default encoding for the writing system represented by the current <see cref="TextInfo"/>. 
        /// This would be equal to encoding for the <see cref="TextInfo.OEMCodePage"/> for non-invariant cultures.
        /// In case the current <see cref="TextInfo"/> represents a culture-invariant writing system, this method
        /// returns <see cref="Encoding.UTF8"/>. 
        /// </summary>
        /// <param name="textInfo">
        /// The <see cref="TextInfo"/> instance to get the encoding for.
        /// </param>
        /// <returns>
        /// A reference to the default encoding for the writing system represented by the current <see cref="TextInfo"/>. 
        /// This would be equal to encoding for the <see cref="TextInfo.OEMCodePage"/> for non-invariant cultures.
        /// In case the current <see cref="TextInfo"/> represents a culture-invariant writing system, this method
        /// returns <see cref="Encoding.UTF8"/>.
        /// </returns>
        /// <seealso cref="Encoding" />
        /// <seealso cref="TextInfo"/>
#if !NETSTANDARD
        /// <seealso cref="TextInfo.OEMCodePage" />
#endif
        /// <seealso cref="System.Globalization.CultureInfo.InvariantCulture" />
        public static Encoding GetEncoding(this TextInfo textInfo)
        {
#if !NETSTANDARD
            return string.IsNullOrEmpty(textInfo.VerifyArgument(nameof(textInfo)).Value.CultureName) ? Encoding.UTF8 : GetOemEncoding(textInfo);
#endif
            return Encoding.UTF8;
        }

#if !NETSTANDARD
        /// <summary>
        /// Gets the encoding for the OEM code page of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </summary>
        /// <param name="textInfo">
        /// The <see cref="TextInfo"/> instance to get the encoding for. 
        /// </param>
        /// <returns>
        /// A reference to the encoding for the OEM code page of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </returns>
        /// <seealso cref="TextInfo.OEMCodePage"/>
        public static Encoding GetOemEncoding(this TextInfo textInfo)
        {
            return Encoding.GetEncoding(textInfo.VerifyArgument(nameof(textInfo)).Value.OEMCodePage);
        }
#endif

#if !NETSTANDARD
        /// <summary>
        /// Gets the encoding for the EBCDIC codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </summary>
        /// <param name="textInfo">
        /// The <see cref="TextInfo"/> instance to get the encoding for. 
        /// </param>
        /// <returns>
        /// A reference to the encoding for the EBCDIC codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </returns>
        /// <seealso cref="TextInfo.EBCDICCodePage"/>
        public static Encoding GetEbcdicEncoding(this TextInfo textInfo)
        {
            return Encoding.GetEncoding(textInfo.VerifyArgument(nameof(textInfo)).Value.EBCDICCodePage);
        }
#endif

#if !NETSTANDARD
        /// <summary>
        /// Gets the encoding for the ANSI codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </summary>
        /// <param name="textInfo">
        /// The <see cref="TextInfo"/> instance to get the encoding for. 
        /// </param>
        /// <returns>
        /// A reference to the encoding for the ANSI codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </returns>
        /// <seealso cref="TextInfo.ANSICodePage"/>
        public static Encoding GetAnsiEncoding(this TextInfo textInfo)
        {
            return Encoding.GetEncoding(textInfo.VerifyArgument(nameof(textInfo)).Value.ANSICodePage);
        }
#endif

#if !NETSTANDARD
        /// <summary>
        /// Gets the encoding for the Mac codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </summary>
        /// <param name="textInfo">
        /// The <see cref="TextInfo"/> instance to get the encoding for. 
        /// </param>
        /// <returns>
        /// A reference to the encoding for the Mac codepage of the writing system represented by the current <see cref="TextInfo"/>. 
        /// </returns>
        /// <seealso cref="TextInfo.MacCodePage"/>
        public static Encoding GetMacEncoding(this TextInfo textInfo)
        {
            return Encoding.GetEncoding(textInfo.VerifyArgument(nameof(textInfo)).Value.MacCodePage);
        }
#endif
    }
}

