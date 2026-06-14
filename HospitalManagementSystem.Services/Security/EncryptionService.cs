using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Security
{
    /// <summary>
    /// Data encryption service using AES-256 algorithm
    /// Used to encrypt sensitive patient and user information
    /// 
    /// Encrypted fields should include:
    /// - Email addresses
    /// - Phone numbers
    /// - Document numbers (Aadhar, Passport, DL, etc.)
    /// - Insurance details
    /// - Emergency contact numbers
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly string _encryptionKey; // Should be 32 characters for 256-bit encryption
        private const int KeySize = 256;
        private const int BlockSize = 128;

        /// <summary>
        /// Initializes the encryption service with encryption key
        /// Key should be stored securely in appsettings or Azure Key Vault
        /// </summary>
        /// <param name="encryptionKey">32-character encryption key (256-bit)</param>
        public EncryptionService(string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(encryptionKey))
                throw new ArgumentException("Encryption key cannot be empty", nameof(encryptionKey));

            if (encryptionKey.Length != 32)
                throw new ArgumentException("Encryption key must be exactly 32 characters (256-bit)", nameof(encryptionKey));

            _encryptionKey = encryptionKey;
        }

        /// <summary>
        /// Encrypts plain text data
        /// Returns Base64 encoded string containing IV + encrypted data
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Base64 encoded encrypted data with IV</returns>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.BlockSize = BlockSize;
                    aes.KeySize = KeySize;

                    // IV is prepended to ciphertext for decryption later
                    var iv = aes.IV;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                    {
                        using (var ms = new MemoryStream())
                        {
                            // Write IV first (not encrypted)
                            ms.Write(iv, 0, iv.Length);

                            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (var sw = new StreamWriter(cs))
                                {
                                    sw.Write(plainText);
                                }

                                return Convert.ToBase64String(ms.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting data", ex);
            }
        }

        /// <summary>
        /// Decrypts Base64 encoded encrypted data
        /// Expects data in format: IV (16 bytes) + encrypted data
        /// </summary>
        /// <param name="encryptedText">Base64 encoded encrypted data with IV</param>
        /// <returns>Decrypted plain text</returns>
        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return encryptedText;

            try
            {
                var buffer = Convert.FromBase64String(encryptedText);

                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.BlockSize = BlockSize;
                    aes.KeySize = KeySize;

                    // Extract IV (first 16 bytes)
                    var iv = new byte[aes.BlockSize / 8];
                    Array.Copy(buffer, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                        {
                            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            {
                                using (var sr = new StreamReader(cs))
                                {
                                    return sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error decrypting data", ex);
            }
        }

        /// <summary>
        /// Generates a random encryption key suitable for AES-256
        /// Use this to generate keys for storage in configuration
        /// </summary>
        /// <returns>32-character encryption key (256-bit as Base64)</returns>
        public static string GenerateEncryptionKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32]; // 32 bytes = 256 bits
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData).Substring(0, 32); // Return 32 chars
            }
        }
    }

    /// <summary>
    /// Interface for data encryption service
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts plain text
        /// </summary>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts encrypted data
        /// </summary>
        string Decrypt(string encryptedText);
    }
}
