using System;
using System.Security.Cryptography;
using System.Text;

namespace Mini_Oyun.Core.Security
{
    public sealed class Sha256CryptoService : ICryptoService
    {
        private readonly string _secretKey;

        public Sha256CryptoService(string secretKey)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        }

        public string ComputeHash(string text)
        {
            if (text == null) return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(text + _secretKey);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);

                string hexString = BitConverter.ToString(hashBytes);

                return hexString.Replace("-", "").ToLowerInvariant();
            }
        }

        public bool VerifyHash(string text, string hash)
        {
            if (string.IsNullOrEmpty(hash)) return false;
            string computed = ComputeHash(text);
            return string.Equals(computed, hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}