﻿using System;
using System.Collections.Generic;
using Axle.Extensions.String;
using Axle.References;
using Axle.Resources.Extraction;


namespace Axle.Resources.Yaml.Extraction
{
    internal sealed class YamlValueExtractor : AbstractResourceExtractor
    {
        private static bool GetYamlFileData(Uri location, out string yamlFileName, out string keyPrefix)
        {
            yamlFileName = keyPrefix = null;
            const string ext = YamlResourceInfo.FileExtension;
            const StringComparison cmp = StringComparison.OrdinalIgnoreCase;
            var locStr = location.ToString();
            keyPrefix = locStr.TakeAfterFirst(ext, cmp);
            yamlFileName = locStr.TakeBeforeFirst(keyPrefix, cmp);

            return !string.IsNullOrEmpty(yamlFileName) && yamlFileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase);
        }

        protected override Nullsafe<ResourceInfo> DoExtract(ResourceContext context, string name)
        {
            if (!GetYamlFileData(context.Location, out var fileName, out var keyPrefix))
            {
                return Nullsafe<ResourceInfo>.None;
            }

            IDictionary<string, string> data;
            var yamlRes = context.ExtractionChain.Extract(fileName);
            if (!yamlRes.HasValue)
            {
                return Nullsafe<ResourceInfo>.None;
            }
            switch (yamlRes.Value)
            {
                case YamlResourceInfo yaml:
                    data = yaml.Data;
                    break;
                default:
                    //using (var stream = yamlRes.Value.Open())
                        //if (stream != null)
                        //{
                        //    var p = new Dictionary<string, string>();
                        //    YamlFileExtractor.ReadData(stream, p);
                        //    data = p;
                        //}
                        //else
                        //{
                            data = new Dictionary<string, string>(YamlFileExtractor.DefaultKeyComparer);
                        //}
                    break;
            }

            if (data != null && data.TryGetValue($"{keyPrefix}{name}", out var result))
            {
                return new TextResourceInfo(name, context.Culture, result);
            }

            return null;
        }
    }
}