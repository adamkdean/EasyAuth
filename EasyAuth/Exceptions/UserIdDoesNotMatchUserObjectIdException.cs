using System;

namespace EasyAuth
{
    /// <summary>
    /// Throw when a user object is passed with the wrong id.
    /// 
    /// For example, you try and update user1 with user2's data:
    /// IUserStore.UpdateUserById(user1.UserId, user2);
    /// Obviously, you can get away with this by changing user2.UserId
    /// but then you are intentionally breaking the system.
    /// </summary>
    [Serializable]
    public class UserIdDoesNotMatchUserObjectIdException : Exception
    {
        public UserIdDoesNotMatchUserObjectIdException() { }        
    }
}