using System;
using System.Linq;
using System.Collections.Generic;
using EasyAuth.Helpers;

namespace EasyAuth.Storage
{
    /// <summary>
    /// Stores users in a list in memory.    
    /// </summary>
    public class MemoryUserStore : IUserStore
    {
        static MemoryUserStore instance = null;
        static readonly object padlock = new object();

        MemoryUserStore() { }

        public static MemoryUserStore Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new MemoryUserStore();
                    return instance;
                }
            }
        }

        public static void Reset()
        {
            instance = null;
        }

        private List<UserData> users = new List<UserData>();

        public void AddUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if(this.UserExistsByUsername(username)) throw new UserAlreadyExistsException();

            UserData user = new UserData
            {
                UserId = users.Count,
                Username = username,
                Password = password
            };
            users.Add(user);
        }

        public void UpdateUserById(int id, UserData newUserData)
        {
            if (newUserData == null) throw new ArgumentNullException();
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();
            if (id != newUserData.UserId) throw new UserIdDoesNotMatchUserObjectIdException();

            UserData oldUserData = this.GetActualUserById(id);
            users.Remove(oldUserData);
            users.Add((UserData)newUserData.Clone()); // we clone this to protect the reference
        }

        public void DeleteUserById(int id)
        {            
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();

            UserData user = this.GetActualUserById(id);
            users.Remove(user);
        }

        public bool UserExistsById(int id)
        {            
            return users.Any(x => x.UserId == id);
        }

        public bool UserExistsByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            return users.Any(x => x.Username == username);
        }

        /// <summary>
        /// Return a clone of the user data object
        /// This is so that they are forced to use the update method
        /// rather than just changing the object directly.
        /// </summary>
        public UserData GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return (UserData)users.First(x => x.UserId == id).Clone();
        }

        /// <summary>
        /// Return a clone of the user data object
        /// This is so that they are forced to use the update method
        /// rather than just changing the object directly.
        /// </summary>
        public UserData GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return (UserData)users.First(x => x.Username == username).Clone();
        }

        /// <summary>
        /// Return the actual object reference for the user
        /// We can use this to search by reference in the List<>
        /// </summary>
        protected UserData GetActualUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return users.First(x => x.UserId == id);
        }

        /// <summary>
        /// Return the actual object reference for the user
        /// We can use this to search by reference in the List<>
        /// </summary>
        protected UserData GetActualUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return users.First(x => x.Username == username);
        }

        public List<UserData> GetAllUsers()
        {
            List<UserData> copiedUsers = GenericCopier<List<UserData>>.DeepCopy(users);
            return copiedUsers;
        }
    }
}