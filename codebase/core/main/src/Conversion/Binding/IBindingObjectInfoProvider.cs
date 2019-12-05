﻿using System;
using Axle.Reflection;

namespace Axle.Conversion.Binding
{
    /// <summary>
    /// An interface representing an object information provider. An object information provider is 
    /// used by a <see cref="IBinder"/> to obtain means for creating instances of a particular object
    /// or to access an object instance's members.
    /// </summary>
    public interface IBindingObjectInfoProvider
    {
        /// <summary>
        /// Finds an object <paramref name="instance"/>'s member with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="instance">
        /// The object instance to find the member on.
        /// </param>
        /// <param name="name">
        /// The name of the member to be found on the object instance.
        /// </param>
        /// <returns>
        /// A <see cref="IReadWriteMember"/> representing the <paramref name="instance"/>'s member with the 
        /// provided <paramref name="name"/>, or <c><see langword="null"/></c> if no such member was found.
        /// </returns>
        /// 
        IReadWriteMember GetMember(object instance, string name);
        /// <summary>
        /// Lists all members of an object <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">
        /// The object instance to find the member on.
        /// </param>
        /// <returns>
        /// An array of <see cref="IReadWriteMember"/> representing all members of the provided object <paramref name="instance"/>.
        /// </returns>
        IReadWriteMember[] GetMembers(object instance);

        /// <summary>
        /// Attempts to create an instance of the provided <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type to create an instance of.
        /// </param>
        /// <param name="instance">
        /// An output parameter representing the newly created object instance.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c><see langword="true"/></c> if an object instance was successfully created, and the 
        /// <paramref name="instance"/> parameter was set;
        /// <c><see langword="false"/></c> otherwise.
        /// </returns>
        bool TryCreateInstance(Type type, out object instance);
    }
}