﻿#if NETSTANDARD || NET20_OR_NEWER
using System;
using System.Collections.Generic;

using Axle.Verification;


namespace Axle
{
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [Serializable]
    #endif
    public class AdaptiveComparer<T1, T2> : IComparer<T1>
    {
        #if !DEBUG
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        #endif
        private readonly Func<T1, T2> _adaptFunc;
        #if !DEBUG
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        #endif
        private readonly IComparer<T2> _actualComparer;

        public AdaptiveComparer(Func<T1, T2> adaptFunc, IComparer<T2> comparer)
        {
            _adaptFunc = Verifier.IsNotNull(Verifier.VerifyArgument(adaptFunc, nameof(adaptFunc)));
            _actualComparer = Verifier.IsNotNull(Verifier.VerifyArgument(comparer, nameof(comparer))).Value;
        }
        public AdaptiveComparer(Func<T1, T2> adaptFunc) : this(adaptFunc, Comparer<T2>.Default){}

        public int Compare(T1 x, T1 y) { return _actualComparer.Compare(_adaptFunc(x), _adaptFunc(y)); }
    }
}
#endif