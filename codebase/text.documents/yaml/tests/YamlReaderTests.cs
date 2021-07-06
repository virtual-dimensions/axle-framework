﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Axle.Text.Documents.Binding;
using NUnit.Framework;

namespace Axle.Text.Documents.Yaml.Tests
{
    [TestFixture]
    public class YamlReaderTests
    {
        private class Wrapper
        {
            public SecureStringHolderWithOtherMembers Nested { get; internal set; }
        }
        private class SecureStringHolderWithOtherMembers : SecureStringHolder
        {
            public long Data { get; internal set; }
            public string OtherData { get; internal set; }
        }
        private class SecureStringHolder
        {
            public SecureString Secret { get; internal set; }
        }
        
        [Test]
        public void TestDataLookup()
        {
            var propertiesPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "data.yaml");
            var reader = new YamlDocumentReader(StringComparer.OrdinalIgnoreCase);
            var data = reader.Read(File.OpenRead(propertiesPath), Encoding.UTF8);

            var item1 = data.GetValues("SingleKey").SingleOrDefault() as ITextDocumentValue;
            var item2 = data.GetValues("System");
            var item3 = data.GetValues("System.Text").OfType<ITextDocumentObject>();
            var item4 = data.GetValues("System.Text.Encoding").FirstOrDefault() as ITextDocumentValue;
            var item5 = data.GetValues("System.Text.DefaultEncoding").FirstOrDefault() as ITextDocumentValue;
            var item6 = item3.Take(1).FirstOrDefault()?.GetValues("Encoding").FirstOrDefault() as ITextDocumentValue;
            var item7 = item3.Take(1).FirstOrDefault()?.GetValues("DefaultEncoding").FirstOrDefault() as ITextDocumentValue;
            
            Assert.IsNotNull(item1, "Lookup for value failed for simple key {0}", "SingleKey");
            Assert.IsNotNull(item2, "Lookup for object failed for simple key {0}", "System");
            Assert.IsNotNull(item3, "Lookup for object failed for complex key {0}", "System.Text");
            Assert.IsTrue(item3.Count() > 1, "Lookup for object failed for complex key {0}", "System.Text");
            Assert.IsNotNull(item4, "Lookup for value failed for complex key {0}", "System.Text.Encoding");
            Assert.IsNotNull(item5, "Lookup for value failed for complex key {0}", "System.Text.DefaultEncoding");
            Assert.IsNotNull(item6, "Lookup for value in object {1} failed for simple key {0}", "Encoding", "System.Text");
            Assert.IsNotNull(item7, "Lookup for value in object {1} failed for simple key {0}", "DefaultEncoding", "System.Text");
        }

        [Test]
        public void TestIfSecureStringMemberCanBeResolved()
        {
            var propertiesPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "data.yaml");
            var reader = new YamlDocumentReader(StringComparer.OrdinalIgnoreCase);
            var data = reader.Read(File.OpenRead(propertiesPath), Encoding.UTF8);
            
            var binder = new DefaultDocumentBinder();
            var secureValueHolder = (SecureStringHolder) binder.Bind(data, new SecureStringHolder());
            Assert.IsNotNull(secureValueHolder);
            Assert.IsNotNull(secureValueHolder.Secret);
            Assert.AreEqual(new[]{'a', 'b', 'c'}.Length, secureValueHolder.Secret.Length);
        }
        
        [Test]
        public void TestIfNestedSecureStringMemberCanBeResolved()
        {
            var propertiesPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "data.yaml");
            var reader = new YamlDocumentReader(StringComparer.OrdinalIgnoreCase);
            var data = reader.Read(File.OpenRead(propertiesPath), Encoding.UTF8);
            
            var binder = new DefaultDocumentBinder();
            var secureValueHolderWrapper = (Wrapper) binder.Bind(data, new Wrapper());
            Assert.IsNotNull(secureValueHolderWrapper);
            Assert.IsNotNull(secureValueHolderWrapper.Nested);
            Assert.IsNotNull(secureValueHolderWrapper.Nested.Secret);
            Assert.AreEqual(new[]{'a', 'b', 'c'}.Length, secureValueHolderWrapper.Nested.Secret.Length);
        }
    }
}
