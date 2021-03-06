﻿using System.Security.Cryptography;

using Axle.Security.Cryptography.Algorithms.Symmetric.Sdk;


namespace Axle.Security.Cryptography.Algorithms.Symmetric
{
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [System.Serializable]
    #endif
    [System.Obsolete("Consider using AES. The DES cipher is considered broken. ")]
    public sealed class DesHashAlgorithm : AbstractSymmetricHashAlgorithm
    {
        public DesHashAlgorithm() : base(new DESCryptoServiceProvider()) { }
    }
}