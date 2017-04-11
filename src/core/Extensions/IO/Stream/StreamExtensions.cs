﻿using System;
using System.IO;

using Axle.Verification;


namespace Axle.Extensions.IO.Stream
{
    using Stream = System.IO.Stream;

    /// <summary>
    /// A static class providing extension methods to <see cref="Stream"/> instances.
    /// </summary>
    public static class StreamExtensions
    {
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Allocates a specified number of bytes from the current position of the target.
        /// <remarks>
        /// Any existing data within the current allocation range will be lost.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The target to allocate bytes</param>
        /// <param name="length">The number of bytes to be allocated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown if the current <see cref="Stream"/> does not support seeking or writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public static void Allocate(this Stream stream, int length)
        {
            stream.VerifyArgument("stream").IsNotNull();
            if (!stream.CanWrite)
            {
                throw new NotSupportedException("The specified input stream does not support writing.");
            }
            var position = stream.Position;
            stream.Write(new byte[length], 0, length);
            stream.Seek(position, SeekOrigin.Begin);
        }

        public static long SeekToBeginning(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            var currentPosition = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            return currentPosition;
        }

        public static long SeekToEnd(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            var currentPosition = stream.Position;
            stream.Seek(0, SeekOrigin.End);
            return currentPosition;
        }

        /// <summary>
        /// Converts the contents of a <see cref="Stream"/> instance to an array of bytes.
        /// </summary>
        /// <param name="stream">The <see cref="Stream">stream</see> to convert.</param>
        /// <param name="bufferSize">An integer value determining the size of the buffer used for reading from the stream.</param>
        /// <param name="leaveOpen">A <see cref="bool">boolean</see> flag indicating wheter to leave the stream open or not. </param>
        /// <returns>
        /// An array of <see cref="byte"/> values representing the contents of the <paramref name="stream"/>.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bufferSize"/> is not a positive number.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public static byte[] ToByteArray(this Stream stream, int bufferSize, bool leaveOpen)
        {
            stream.VerifyArgument(nameof(stream)).IsNotNull();
            bufferSize.VerifyArgument(nameof(bufferSize)).IsGreaterThan(0);

            var result = new byte[stream.Length];
            var position = stream.Position;
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                var loc = 0;
                do
                {
                    var available = result.Length - loc;
                    var count = available > bufferSize ? bufferSize : available;
                    loc += stream.Read(result, loc, count);
                }
                while (loc < result.Length);
            }
            finally
            {
                if (leaveOpen)
                {
                    stream.Seek(position, SeekOrigin.Begin);
                }
                else
                {
                    stream.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// Converts the contents of a <see cref="Stream"/> instance to an array of bytes.
        /// </summary>
        /// <param name="stream">The <see cref="Stream">stream</see> to convert.</param>
        /// <param name="leaveOpen">A <see cref="bool">boolean</see> flag indicating wheter to leave the stream open or not. </param>
        /// <returns>
        /// An array of <see cref="byte"/> values representing the contents of the <paramref name="stream"/>.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public static byte[] ToByteArray(this Stream stream, bool leaveOpen) { return ToByteArray(stream, DefaultBufferSize, leaveOpen); }
        /// <summary>
        /// Converts the contents of a <see cref="Stream"/> instance to an array of bytes.
        /// <remarks>
        /// This method will not close the <paramref name="stream"/>.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The <see cref="Stream">stream</see> to convert.</param>
        /// <param name="bufferSize">An integer value determining the size of the buffer used for reading from the stream.</param>
        /// <returns>
        /// An array of <see cref="byte"/> values representing the contents of the <paramref name="stream"/>.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public static byte[] ToByteArray(this Stream stream, int bufferSize) { return ToByteArray(stream, bufferSize, false); }
        /// <summary>
        /// Converts the contents of a <see cref="Stream"/> instance to an array of bytes.
        /// <remarks>
        /// This method will not close the <paramref name="stream"/>.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The <see cref="Stream">stream</see> to convert.</param>
        /// <returns>
        /// An array of <see cref="byte"/> values representing the contents of the <paramref name="stream"/>.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public static byte[] ToByteArray(this Stream stream) { return ToByteArray(stream, DefaultBufferSize, false); }

        #if net45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static long WriteToUnchecked(Stream stream, Stream target, byte[] buffer)
        {
            var i = stream.Read(buffer, 0, buffer.Length);
            long size = i;
            do
            {
                target.Write(buffer, 0, i);
                i = stream.Read(buffer, 0, buffer.Length);
                size += i;
            }
            while (i > 0);
            return size;
        }

        /// <summary>
        /// Writes data to an output target using the specified input target and buffer size.
        /// <remarks>
        /// The data is being read from the input target's current position.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        /// <param name="target">The target to write data to.</param>
        /// <param name="bufferSize">The size of the buffer (byte array) that will be used for writing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The current input target does not support reading.</exception>
        /// <exception cref="NotSupportedException">The specified output target does not support writing.</exception>
        /// <exception cref="InvalidOperationException">The output target and the input target are the same instance.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Parameter <paramref name="bufferSize"/> is not a positive number. </exception>
        public static long WriteTo(this Stream stream, Stream target, int bufferSize)
        {
            stream.VerifyArgument ("stream").IsNotNull();
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, "Invalid buffer size");
            }
            var buffer = new byte[bufferSize];
            return WriteTo(stream, target, buffer);
        }
        /// <summary>
        /// Writes data to an output target using the specified input target and buffer size.
        /// <remarks>
        /// The data is being read from the input target's current position.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        /// <param name="target">The target to write data to.</param>
        /// <param name="buffer">A byte array to be used the buffer for the write operation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The current input target does not support reading.</exception>
        /// <exception cref="NotSupportedException">The specified output target does not support writing.</exception>
        /// <exception cref="InvalidOperationException">The output target and the input target are the same instance.</exception>
        public static long WriteTo(this Stream stream, Stream target, byte[] buffer)
        {
            stream.VerifyArgument("stream").IsNotNull();
            if (!stream.CanRead)
            {
                throw new NotSupportedException("The specified input stream does not support reading.");
            }
            if (!target.CanWrite)
            {
                throw new NotSupportedException("The specified output stream does not support writing.");
            }
            if (ReferenceEquals(target, stream))
            {
                throw new InvalidOperationException("The output stream cannot be the same instance as the specified input stream.");
            }
            return WriteToUnchecked(stream, target, buffer);
        }
    }
}
