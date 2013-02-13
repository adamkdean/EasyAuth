using System;
namespace EasyAuth
{
    public class User : ICloneable
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}