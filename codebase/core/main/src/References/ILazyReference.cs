﻿#if NETSTANDARD || NET40_OR_NEWER
namespace Axle.References
{
    /// <summary>
    /// An interface representing a reference to a lazily initialized value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value that is lazy-initialized.
    /// </typeparam>
    /// <seealso cref="System.Lazy{T}"/>
    public interface ILazyReference<T> : IReference<T>
    {
        /// <summary>
        /// Tries to retrieve the value that is referenced by the current <see cref="ILazyReference{T}"/> object.
        /// <remarks>
        /// Calling this method will not enforce the lazy value initialization.
        /// </remarks>
        /// </summary>
        /// <param name="value">
        /// When this method returns, contains the reference value, if it is available.
        /// This parameter is treated as uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the target value was retrieved; <c>false</c> otherwise.
        /// </returns>
        new bool TryGetValue(out T value);

        /// <summary>
        /// Gets a boolean value that indicates whether the current
        /// <see cref="ILazyReference{T}"/> has a <see cref="IReference{T}.Value"/> created.
        /// </summary>
        bool HasValue { get; }
    }
}
#endif