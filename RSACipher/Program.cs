using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RSACipher
{
    class Program
    {
        private static KeyGenerator keyGenerator;
        private static RSAEncrypter encrypter;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Application is loading... Please wait");

            keyGenerator = new KeyGenerator();
            encrypter = new RSAEncrypter();

            var filePath = GetFilePath();
            var decryptedFilePath = GetDecryptedFilePath(filePath);

            var image = await File.ReadAllBytesAsync(filePath);

            var encryptedImage = GetEncryptedImage(image);
            var decryptedImage = GetDecryptedImage(encryptedImage);

            await File.WriteAllBytesAsync(decryptedFilePath, decryptedImage);
        }

        private static byte[] GetDecryptedImage(IReadOnlyCollection<BigInteger> encryptedImage)
        {
            var privateKey = keyGenerator.GetPrivateKey();
            return encrypter.Decrypt(encryptedImage, privateKey);
        }

        private static IReadOnlyCollection<BigInteger> GetEncryptedImage(byte[] originalImage)
        {
            var publicKey = keyGenerator.GetPublicKey();
            return encrypter.Encrypt(originalImage, publicKey);
        }

        private static string GetDecryptedFilePath(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return filePath.Replace(fileName, fileName + "-decrypted");
        }

        private static string GetFilePath()
        {
            string filePath;

            do
            {
                Console.WriteLine("Please give file path");
                filePath = Console.ReadLine();
            } while (!File.Exists(filePath));

            return filePath;
        }
    }
}
