﻿using System;
using System.Collections.Generic;


namespace Axle.References
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> implementation that can compare weak references. 
    /// Two weak references are deemed equal in case both are not alive or in case the targets of both are considered equal by the <see cref="ValueComparer"/>. 
    /// </summary>
    /// <typeparam name="T">
    /// The underlying type of the weak reference. Must be a reference type. 
    /// </typeparam>
    /// <seealso cref="IEqualityComparer{T}"/>
    [Serializable]
    public sealed class WeakReferenceEqualityComparer<T> : AbstractEqualityComparer<IWeakReference<T>> where T: class 
    {
        private readonly IEqualityComparer<T> valueComparer;
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferenceEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="valueComparer">
        /// The <see cref="IEqualityComparer{T}"/> that is used to compare the weakreference values.
        /// </param>
        public WeakReferenceEqualityComparer(IEqualityComparer<T> valueComparer) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferenceEqualityComparer{T}"/> class that compares the weak reference targets by reference.
        /// The <see cref="ValueComparer"/> implementation will be <see cref="ReferenceEqualityComparer{T}"/>. 
        /// </summary>
        /// <seealso cref="ReferenceEqualityComparer{T}"/>
        public WeakReferenceEqualityComparer() : this(new ReferenceEqualityComparer<T>()) { }

        protected override int DoGetHashCode(IWeakReference<T> obj)
        {
            return obj != null ? obj.GetHashCode() : 0;
        }

        protected override bool DoEquals(IWeakReference<T> x, IWeakReference<T> y)
        {
            var xTarget = x.Value;
            var yTarget = y.Value;

            if (! x.IsAlive && ! y.IsAlive) 
            {
                return true;
            }

            if (! (ReferenceEquals(xTarget, null) || ReferenceEquals(yTarget, null))) 
            {
                return valueComparer.Equals(xTarget, yTarget);
            }

            return false;
        }

        /// <summary>
        /// Gets a reference to the <see cref="IEqualityComparer{T}"/> that is used to compare the weakreference values.
        /// </summary>
        /// <value>The value comparer.</value>
        public IEqualityComparer<T> ValueComparer { get{ return valueComparer; } }
    }
}