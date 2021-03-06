﻿using System;


namespace Axle.Reflection
{
    /// <summary>
    /// An enumeration representing the possible declaration types of a class member.
    /// </summary>
    /// <remarks>
    /// Multiple values can be combined, as the enumeration has the <see cref="FlagsAttribute"/> specified.
    /// </remarks>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [Serializable]
    #endif
    [Flags]
    public enum DeclarationType : byte
    {
        /// <summary>
        /// No valid declaration type is specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Used for instance members of a class.
        /// </summary>
        Instance = 1,

        /// <summary>
        /// Used for <see langword="static"/> members of a class.
        /// </summary>
        /// <seealso cref="System.Reflection.MethodBase.IsStatic"/>
        /// <seealso cref="System.Reflection.FieldInfo.IsStatic"/>
        Static = (1 << 1),

        /// <summary>
        /// The member is <see langword="abstract"/>.
        /// </summary>
        /// <seealso cref="System.Reflection.MethodBase.IsAbstract"/>
        Abstract = (1 << 2),

        /// <summary>
        /// The member is <see langword="virtual"/>.
        /// </summary>
        /// <seealso cref="System.Reflection.MethodBase.IsVirtual"/>
        Virtual = (1 << 3),

        /// <summary>
        /// The member is overriding another member defined up in the class hierarchy.
        /// </summary>
        Override = (1 << 4),

        /// <summary>
        /// Determines that only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        /// <seealso cref="System.Reflection.MethodBase.IsHideBySig"/>
        HideBySig = (1 << 5),

        /// <summary>
        /// The member is <see langword="sealed"/>; it cannot be overridden. 
        /// </summary>
        /// <seealso cref="System.Reflection.MethodBase.IsFinal"/>
        Sealed = (1 << 6)
    }
}