﻿using Axle.Text.Documents;
using Axle.Verification;

namespace Axle.Configuration.Text.Documents
{
    /// <summary>
    /// An implementation of the <see cref="AbstractTextDocumentConfigSource"/> class which reads the underlying
    /// <see cref="ITextDocumentRoot"/> from a text value.
    /// </summary>
    public sealed class TextDocumentConfigSource : AbstractTextDocumentConfigSource
    {
        private readonly ITextDocumentRoot _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDocumentConfigSource"/> class, using the
        /// provided document <paramref name="reader"/> and <paramref name="rawDocument"/> string.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="ITextDocumentReader"/> instance that will be used to load the <see cref="ITextDocumentRoot"/>
        /// representing the configuration.
        /// </param>
        /// <param name="rawDocument">
        /// A <see cref="string"/> representation of the raw contents for the <see cref="ITextDocumentRoot"/> to be
        /// loaded by the <paramref name="reader"/>.
        /// </param>
        public TextDocumentConfigSource(ITextDocumentReader reader, string rawDocument)
        {
            Verifier.IsNotNull(Verifier.VerifyArgument(reader, nameof(reader)));
            Verifier.IsNotNull(Verifier.VerifyArgument(rawDocument, nameof(rawDocument)));
            _document = reader.Read(rawDocument);
        }

        /// <inheritdoc />
        protected override ITextDocumentRoot ReadDocument() => _document;
    }
}