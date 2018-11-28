using Org.BouncyCastle.Math;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RSACipher
{
    public class RSAEncrypter
    {
        private const int BatchSize = 255;

        public IReadOnlyCollection<BigInteger> Encrypt(byte[] message, PublicKey key)
        {
            var batchedMessage = message.Batch(BatchSize).ToList();
            var encryptedMessage = batchedMessage.Select(batch => EncryptBatch(batch, key)).ToList();

            return encryptedMessage;
        }

        public byte[] Decrypt(IReadOnlyCollection<BigInteger> batchedCipher, PrivateKey key)
        {
            var decryptedMessage = batchedCipher.Select(batch => DecryptBatch(batch, key)).ToList();

            var bytes = new List<byte>();
            for (var i = 0; i < decryptedMessage.Count; i++)
            {
                var batch = decryptedMessage.ElementAt(i);
                var batchBytes = batch.ToByteArrayUnsigned();

                if (batchBytes.Length < BatchSize && !IsLastBatch(i, decryptedMessage))
                {
                    batchBytes = GetLeftPadded(batchBytes);
                }

                bytes.AddRange(batchBytes);
            }

            return bytes.ToArray();
        }

        private static byte[] GetLeftPadded(IReadOnlyCollection<byte> batchBytes)
        {
            var missingBytes = BatchSize - batchBytes.Count;
            var padding = Enumerable.Repeat(new byte(), missingBytes).ToList();

            return padding.Concat(batchBytes).ToArray();
        }

        private static bool IsLastBatch(int index, ICollection decryptedMessage)
        {
            return index == decryptedMessage.Count - 1;
        }

        private static BigInteger EncryptBatch(BigInteger batch, PublicKey key)
        {
            return batch.ModPow(key.E, key.N);
        }

        private static BigInteger DecryptBatch(BigInteger batch, PrivateKey key)
        {
            return batch.ModPow(key.D, key.N);
        }
    }
}