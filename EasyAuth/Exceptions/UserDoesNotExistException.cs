using System;

namespace EasyAuth
{
    [Serializable]
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() { }        
    }
}