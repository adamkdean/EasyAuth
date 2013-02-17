using System;
namespace EasyAuth
{
    [Serializable]
    public class User : ICloneable
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}