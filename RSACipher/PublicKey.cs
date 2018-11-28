using Org.BouncyCastle.Math;

namespace RSACipher
{
    public class PublicKey
    {
        public PublicKey(BigInteger n, BigInteger e)
        {
            N = n;
            E = e;
        }

        public BigInteger N { get; }
        public BigInteger E { get; }
    }
}