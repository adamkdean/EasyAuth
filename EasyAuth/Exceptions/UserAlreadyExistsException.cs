using System;

namespace EasyAuth
{
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() { }
    }
}