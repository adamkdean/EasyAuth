using System;
using System.Linq;
using System.Security.Cryptography;

namespace EasyAuth.Security
{
    public class SHA256L100HashProvider : HashProvider
    {
        private const int hashLoops = 100; // SHA256 L100
        private SHA256Managed hashAlgorithm = new SHA256Managed();

        public override int HashLength
        {
            get { return hashAlgorithm.HashSize; }
        }

        public override byte[] GetSalt()
        {
            return GetSalt(SaltLength);
        }

        public override byte[] GetSalt(int length)
        {
            var buffer = new byte[length];
            Random random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(buffer);
            return buffer;
        }

        public override byte[] GetHash(byte[] data, byte[] salt)
        {
            var combined = data.Concat(salt).ToArray();
            var hash = hashAlgorithm.ComputeHash(combined);
            for (int i = 0; i < hashLoops - 1; i++)
                hash = hashAlgorithm.ComputeHash(hash);            
            return hash;
        }

        public override string GetHash(string data, byte[] salt)
        {
            var dataBytes = GetBytes(data);
            var hashBytes = GetHash(dataBytes, salt);
            var hashString = GetString(hashBytes);
            return hashString;
        }
    }
}