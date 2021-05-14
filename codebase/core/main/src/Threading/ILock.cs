﻿using System;

namespace Axle.Threading
{
}


namespace Axle.Threading
{
    /// <summary>
    /// An interface representing a synchronization lock construct.
    /// </summary>
    public interface ILock
    {
        /// <summary>
        /// Acquires a lock and creates a <see cref="IDisposable">disposable</see> <see cref="ILockHandle"/> object to
        /// control the duration of the obtained lock.
        /// </summary>
        /// <returns>
        /// A <see cref="ILockHandle"/> instance.
        /// </returns>
        ILockHandle CreateHandle();
        
        /// <summary>
        /// Acquires an exclusive lock.
        /// </summary>
        void Enter();

        /// <summary>
        /// Releases a previously obtained exclusive lock.
        /// </summary>
        void Exit();

        /// <summary>
        /// Attempts to acquire an exclusive lock.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait for the lock.
        /// A value of <c>-1</c> represents an infinite wait.
        /// </param>
        /// <seealso cref="System.Threading.Timeout.Infinite"/>
        /// <returns>
        /// <c>true</c> if the lock was acquired successfully; <c>false</c> otherwise.
        /// </returns>
        bool TryEnter(int millisecondsTimeout);

        /// <summary>
        /// Attempts to acquire an exclusive lock.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> representing the amount of time to wait for the lock.
        /// </param>
        /// <returns>
        /// <c>true</c> if the lock was acquired successfully; <c>false</c> otherwise.
        /// </returns>
        bool TryEnter(TimeSpan timeout);
    }
}