﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Axle.Text.Data.Properties.Tests
{
    [TestFixture]
    public class PropertiesReaderTests
    {
        [Test]
        public void TestDataLookup()
        {
            var propertiesPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "data.properties");
            var reader = new PropertiesDataReader(StringComparer.OrdinalIgnoreCase);
            var data = reader.Read(File.OpenRead(propertiesPath), Encoding.UTF8);

            var item1 = data.GetChildren("SingleKey").SingleOrDefault() as ITextDataValue;
            var item2 = data.GetChildren("System");
            var item3 = data.GetChildren("System.Text");
            var item4 = data.GetChildren("System.Text.Encoding").SingleOrDefault() as ITextDataValue;
            var item5 = data.GetChildren("System.Text.DefaultEncoding").SingleOrDefault() as ITextDataValue;
            var item6 = item3.OfType<ITextDataObject>().Take(1).SingleOrDefault()?.GetChildren("Encoding").SingleOrDefault() as ITextDataValue;
            var item7 = item3.OfType<ITextDataObject>().Skip(1).Take(1).SingleOrDefault()?.GetChildren("DefaultEncoding").SingleOrDefault() as ITextDataValue;
            
            Assert.IsNotNull(item1, "Lookup for value failed for simple key {0}", "SingleKey");
            Assert.IsNotNull(item2, "Lookup for object failed for simple key {0}", "System");
            Assert.IsNotNull(item3, "Lookup for object failed for complex key {0}", "System.Text");
            Assert.IsNotNull(item4, "Lookup for value failed for complex key {0}", "System.Text.Encoding");
            Assert.IsNotNull(item5, "Lookup for value failed for complex key {0}", "System.Text.DefaultEncoding");
            Assert.IsNotNull(item6, "Lookup for value in object {1} failed for simple key {0}", "Encoding", "System.Text");
            Assert.IsNotNull(item7, "Lookup for value in object {1} failed for simple key {0}", "DefaultEncoding", "System.Text");
        }
    }
}
