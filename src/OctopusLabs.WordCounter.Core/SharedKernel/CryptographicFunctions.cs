using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Core.SharedKernel
{
    public static class CryptographicFunctions
    {
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes =
                new byte[plainText.Length + salt.Length];

            for (var i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (var i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static byte[] Encrypt(string publicKey, string text)
        {
            // Convert the text to an array of bytes   
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(text);

            // Create a byte array to store the encrypted data in it   
            byte[] encryptedData;
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Set the rsa pulic key   
                rsa.FromXMLString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, false);
            }

            return encryptedData;
        }

        public static string Decrypt(string privateKey, byte[] dataToDecrypt)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Create an array to store the decrypted data in it   
                // Set the private key of the algorithm   
                rsa.FromXMLString(privateKey);
                var decryptedData = rsa.Decrypt(dataToDecrypt, false);

                // Get the string value from the decryptedData byte array   
                var byteConverter = new UnicodeEncoding();
                return byteConverter.GetString(decryptedData);
            }
        }

        public static RSACryptoServiceProvider GetKeyFromContainer(string containerName)
        {
            var cp = CreateCspParameters(containerName);

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            var rsa = new RSACryptoServiceProvider(cp);

            return rsa;
        }

        private static CspParameters CreateCspParameters(string containerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            return new CspParameters
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
        }
    }
}
