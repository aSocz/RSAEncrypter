using Org.BouncyCastle.Math;

namespace RSACipher
{
    public class PrivateKey
    {
        public PrivateKey(BigInteger n, BigInteger d)
        {
            N = n;
            D = d;
        }

        public BigInteger N { get; }
        public BigInteger D { get; }
    }
}