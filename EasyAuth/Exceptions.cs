using System;

namespace EasyAuth
{
    /// <summary>
    /// Thrown when a user does not exists
    /// </summary>
    [Serializable]
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() { }
        public UserDoesNotExistException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when a user already exists
    /// </summary>
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() { }
        public UserAlreadyExistsException(string message) : base(message) { }        
    }

    /// <summary>
    /// Throw when a user object is passed with the wrong id.
    /// 
    /// For example, you try and update user1 with user2's data:
    /// IUserStore.UpdateUserById(user1.UserId, user2);
    /// Obviously, you can get away with this by changing user2.UserId
    /// but then you are intentionally breaking the system.
    /// </summary>
    [Serializable]
    public class UserIdDoesNotMatchUserObjectId : Exception
    {
        public UserIdDoesNotMatchUserObjectId() { }
        public UserIdDoesNotMatchUserObjectId(string message) : base(message) { }
    }
}