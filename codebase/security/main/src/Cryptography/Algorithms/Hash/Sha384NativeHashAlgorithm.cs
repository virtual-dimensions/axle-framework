#if NETFRAMEWORK && NET35_OR_NEWER
using System.Security.Cryptography;

namespace Axle.Security.Cryptography.Algorithms.Hash
{
    public sealed class Sha384NativeHashAlgorithm : AbstractHashAlgorithm
    {
        public Sha384NativeHashAlgorithm() : base(new SHA384CryptoServiceProvider()) { }
    }
}
#endif