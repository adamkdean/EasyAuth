using System;
using System.Text;

namespace EasyAuth.Security
{
    public abstract class HashProvider
    {
        public virtual int SaltLength { get; set; }
        public abstract int HashLength { get; }

        protected HashProvider()
        {
            SaltLength = 8;
        }

        public abstract byte[] GetSalt();
        public abstract byte[] GetSalt(int length);
        public abstract byte[] GetHash(byte[] data, byte[] salt);
        public abstract string GetHash(string data, byte[] salt);

        public virtual byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);            
        }

        public virtual string GetString(byte[] bytes)
        {
            return BitConverter.ToString(bytes)
                               .Replace("-", "")
                               .ToLowerInvariant();
        }
    }
}