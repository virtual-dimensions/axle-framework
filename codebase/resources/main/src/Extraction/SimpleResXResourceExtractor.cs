﻿#if !NETSTANDARD || NETSTANDARD2_0_OR_NEWER
using System;
using System.Globalization;
using System.IO;

using Axle.Extensions.Uri;


namespace Axle.Resources.Extraction
{
    /// <summary>
    /// The .NET's native resource extractor implementation, supporting only strings and streamed resources.
    /// </summary>
    /// <remarks>
    /// This implementation does not depend on the <see cref="System.Drawing">System.Drawing</see> assembly.
    /// </remarks>
    public sealed class SimpleResXResourceExtractor : AbstractResXResourceExtractor
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SimpleResXResourceExtractor"/> class.
        /// </summary>
        /// <param name="type">
        /// The type that represents the .NET resource container.
        /// </param>
        public SimpleResXResourceExtractor(Type type, Uri baseUri) : base(type, baseUri)
        {
        }

        protected override ResourceInfo ExtractResource(ResXResourceResolver resolver, string name, CultureInfo culture)
        {
            var location = BaseUri.Resolve($"./{name}");
            switch (resolver.Resolve(location, culture))
            {
                case string str:
                    return new TextResourceInfo(BaseUri, name, culture, str);
                case Stream stream:
                    // We do not need the actual stream here, we only used it to determine the resource type.
                    stream.Dispose();
                    // Create a resource representation that will always open a fresh stream when the underlying data is requested.
                    // This will avoid issues when the resource is latter being marshalled to another form.
                    var result = new ResXStreamResourceInfo(resolver, BaseUri,  name, culture);
                    // return the resource
                    return result;
                default:
                    return null;
            }
        }
    }
}
#endif