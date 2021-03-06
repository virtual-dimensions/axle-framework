﻿namespace Axle.Text.Data.Binding
{
    /// <summary>
    /// An interface representing an binder object provider; that is a type of <see cref="IBoundValueProvider"/> 
    /// that is used to supply values for complex types.
    /// </summary>
    public interface IBoundComplexValueProvider : IBoundValueProvider
    {
        /// <summary>
        /// Attempts to get the value(s) associated for the given <paramref name="member"/>.
        /// </summary>
        /// <param name="member">
        /// The member to lookup values for.
        /// </param>
        /// <param name="value">
        /// An <see cref="IBoundValueProvider"/> instance representing the value associated with the given 
        /// <paramref name="member"/>. 
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c>, if there are values for the given <paramref name="member"/>; <c>false</c> otherwise.
        /// </returns>
        bool TryGetValue(string member, out IBoundValueProvider value);

        /// <summary>
        /// Gets the value(s) associated for the given <paramref name="member"/>.
        /// </summary>
        /// <param name="member">
        /// The member to lookup values for.
        /// </param>
        /// <returns>
        /// An <see cref="IBoundValueProvider"/> instance representing the values associated with the given 
        /// <paramref name="member"/>, or <c><see langword="null"/></c> if no values exist for the given member.
        /// </returns>
        IBoundValueProvider this[string member] { get; }
    }
}
