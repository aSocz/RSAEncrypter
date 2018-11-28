using Org.BouncyCastle.Math;
using System;

namespace RSACipher
{
    public class KeyGenerator
    {
        private readonly BigInteger fi;
        private readonly BigInteger n;
        private readonly Random rand;

        private BigInteger e;


        public KeyGenerator()
        {
            rand = new Random();

            var p = GetProbablePrime();
            var q = GetProbablePrime();

            n = p.Multiply(q);
            fi = p.Subtract(BigInteger.One).Multiply(q.Subtract(BigInteger.One));

            SetEulerTotient();
        }

        public PublicKey GetPublicKey() => new PublicKey(n, e);

        public PrivateKey GetPrivateKey()
        {
            var d = e.ModInverse(fi);
            return new PrivateKey(n, d);
        }

        private void SetEulerTotient()
        {
            var minValue = BigInteger.Two;

            do
            {
                e = Extensions.GetRandom(minValue, fi);
            } while (!Equals(e.Gcd(fi), BigInteger.One));
        }

        private BigInteger GetProbablePrime(int bits = 1024)
        {
            BigInteger prime;
            do
            {
                prime = BigInteger.ProbablePrime(bits, rand);
            } while (!Equals(prime.Mod(new BigInteger("4")), BigInteger.Three));

            return prime;
        }
    }
}