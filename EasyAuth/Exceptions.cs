using System;

namespace EasyAuth
{
    [Serializable]
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() { }
        public UserDoesNotExistException(string message) : base(message) { }
    }

    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() { }
        public UserAlreadyExistsException(string message) : base(message) { }        
    }

    [Serializable]
    public class UserIdDoesNotMatchUserObjectId : Exception
    {
        public UserIdDoesNotMatchUserObjectId() { }
        public UserIdDoesNotMatchUserObjectId(string message) : base(message) { }
    }
}