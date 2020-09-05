﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Axle.IO.Extensions.Stream;
using Axle.Verification;
#if NETSTANDARD && !NETSTANDARD1_3_OR_NEWER
using Axle.Text.Extensions.Encoding;
#endif

namespace Axle.Text.Documents
{
    /// <summary>
    /// An abstract class aiding in the implementation of the <see cref="ITextDocumentReader"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class AbstractTextDocumentReader : ITextDocumentReader
    {
        private static IEnumerable<ITextDocumentNode> FixHierarchy(string key, IEnumerable<ITextDocumentNode> nodes)
        {
            var tokenizedKey = TextDocumentObject.Tokenize(key);
            var currentNodes = nodes;
            if (tokenizedKey.Length > 1)
            {
                var fixedKey = tokenizedKey[tokenizedKey.Length - 1];
                currentNodes = Enumerable.Select(currentNodes, node =>
                {
                    switch (node)
                    {
                        case ITextDocumentValue v:
                            return new TextDocumentValue(fixedKey, null, v.Value);
                        case ITextDocumentObject o:
                            return new TextDocumentObject(fixedKey, null, o.GetChildren(), false);
                        default:
                            return node;
                    }
                });
            }
            for (var i = tokenizedKey.Length - 2; i >= 0; i--)
            {
                currentNodes = new[] {new TextDocumentObject(tokenizedKey[i], null, currentNodes, false)};
            }
            return currentNodes;
        }

        protected AbstractTextDocumentReader(IEqualityComparer<string> comparer)
        {
            Verifier.IsNotNull(Verifier.VerifyArgument(comparer, nameof(comparer)));
            Comparer = comparer;
        }

        protected virtual ITextDocumentAdapter CreateAdapter(Stream stream, Encoding encoding) 
            => CreateAdapter(encoding.GetString(StreamExtensions.ToByteArray(stream)));
        protected abstract ITextDocumentAdapter CreateAdapter(string data);
        
        private ITextDocumentRoot ReadStructuredData(
            ITextDocumentAdapter adapter, 
            IEqualityComparer<string> comparer)
        {
            return new TextDocumentRoot(ReadStructuredData(string.Empty, adapter), comparer);
        }

        private IEnumerable<ITextDocumentNode> ExpandChildren(ITextDocumentAdapter adapter)
        {
            foreach (var childGroup in Enumerable.GroupBy(adapter.Children, x => x.Key, Comparer))
            foreach (var child in childGroup)
            foreach (var node in FixHierarchy(childGroup.Key, ReadStructuredData(childGroup.Key, child)))
            {
                yield return node;
            }
        }
        
        private IEnumerable<ITextDocumentNode> ReadStructuredData(string key, ITextDocumentAdapter adapter)
        {
            var isRoot = key.Length == 0;
            if (isRoot)
            {
                foreach (var node in ExpandChildren(adapter))
                {
                    yield return node;
                }
            }
            else
            {
                if (adapter.Value != null)
                {
                    yield return new TextDocumentValue(key, null, adapter.Value);
                }
                var children = ExpandChildren(adapter);
                if (Enumerable.Any(children))
                {
                    yield return new TextDocumentObject(key, null, children, false);
                }
            }
        }

        public ITextDocumentRoot Read(Stream stream, Encoding encoding)
        {
            Verifier.IsNotNull(Verifier.VerifyArgument(stream, nameof(stream)));
            Verifier.IsNotNull(Verifier.VerifyArgument(encoding, nameof(encoding)));
            return ReadStructuredData(CreateAdapter(stream, encoding), Comparer);
        }

        public ITextDocumentRoot Read(string data)
        {
            Verifier.IsNotNull(Verifier.VerifyArgument(data, nameof(data)));
            return ReadStructuredData(CreateAdapter(data), Comparer);
        }

        protected IEqualityComparer<string> Comparer { get; }
    }
}