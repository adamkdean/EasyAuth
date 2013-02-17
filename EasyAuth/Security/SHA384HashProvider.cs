using System;
using System.Linq;
using System.Security.Cryptography;

namespace EasyAuth.Security
{
    public class SHA384HashProvider : HashProvider
    {
        private SHA384Managed hashAlgorithm = new SHA384Managed();

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
            return hashAlgorithm.ComputeHash(combined);
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