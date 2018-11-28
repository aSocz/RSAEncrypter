using Org.BouncyCastle.Math;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace RSACipher
{
    public static class Extensions
    {
        // taken from https://stackoverflow.com/questions/42805544/generate-a-random-biginteger-between-two-values-c-sharp
        public static BigInteger GetRandom(BigInteger min, BigInteger max)
        {
            var rng = new RNGCryptoServiceProvider();

            // shift to 0...max-min
            var max2 = max.Subtract(min);

            var bits = max2.BitCount;

            // 1 bit for sign (that we will ignore, we only want positive numbers!)
            bits++;

            // we round to the next byte
            var bytes = (bits + 7) / 8;

            var uselessBits = bytes * 8 - bits;

            var bytes2 = new byte[bytes];

            while (true)
            {
                rng.GetBytes(bytes2);

                // The maximum number of useless bits is 1 (sign) + 7 (rounding) == 8
                if (uselessBits == 8)
                {
                    // and it is exactly one byte!
                    bytes2[0] = 0;
                }
                else
                {
                    // Remove the sign and the useless bits
                    for (var i = 0; i < uselessBits; i++)
                    {
                        //Equivalent to
                        //byte bit = (byte)(1 << (7 - (i % 8)));
                        var bit = (byte)(1 << (7 & ~i));

                        //Equivalent to
                        //bytes2[i / 8] &= (byte)~bit;
                        bytes2[i >> 3] &= (byte)~bit;
                    }
                }

                var bi = new BigInteger(bytes2);

                // If it is too much big, then retry!
                if (bi.CompareTo(max2) >= 0)
                {
                    continue;
                }

                // unshift the number
                bi = bi.Add(min);
                return bi;
            }
        }

        public static IEnumerable<BigInteger> Batch(this IEnumerable<byte> source, int size)
        {
            byte[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new byte[size];
                }

                bucket[count++] = item;
                if (count != size)
                {
                    continue;
                }

                yield return new BigInteger(1, bucket);

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                yield return new BigInteger(1, bucket.Take(count).ToArray());
            }
        }
    }
}