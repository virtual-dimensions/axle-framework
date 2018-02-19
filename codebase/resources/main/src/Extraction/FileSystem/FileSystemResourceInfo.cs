﻿#if !NETSTANDARD || NETSTANDARD1_3_OR_NEWER
using System;
using System.Globalization;
using System.IO;

using Axle.Extensions.Uri;
using Axle.Verification;


namespace Axle.Resources.Extraction.FileSystem
{
    /// <inheritdoc />
    /// <summary>
    /// A class representing a resource located on the file system.
    /// </summary>
    public class FileSystemResourceInfo : ResourceInfo
    {
        private readonly Uri _location;

        /// <inheritdoc />
        internal FileSystemResourceInfo(Uri location, string name, CultureInfo culture) : this(location, name, culture, "application/octet-stream") { }
        internal FileSystemResourceInfo(Uri location, string name, CultureInfo culture, string contentType) : base(name, culture, contentType)
        {
            _location = location.VerifyArgument(nameof(location)).IsNotNull().Value.Resolve(name);
        }

        /// <inheritdoc />
        public override Stream Open()
        {
            return new FileStream(_location.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
#endif