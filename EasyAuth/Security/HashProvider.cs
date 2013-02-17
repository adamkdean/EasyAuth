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

        public abstract string GetSalt();
        public abstract string GetSalt(int length);
        public abstract string GetHash(string data, string salt);
    }
}