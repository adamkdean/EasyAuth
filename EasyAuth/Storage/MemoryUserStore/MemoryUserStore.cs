using System;
using System.Linq;
using System.Collections.Generic;
using EasyAuth.Helpers;
using System.Runtime.CompilerServices;
using EasyAuth.Security;

/* We only want the Tests to be able to call IUserStore.Reset() */
[assembly: InternalsVisibleTo("EasyAuth.Tests")]

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

        private List<User> users = new List<User>();

        public void AddUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (this.UserExistsByUsername(username)) throw new UserAlreadyExistsException();

            var hashProvider = (HashProvider)Activator.CreateInstance(Authentication.HashProviderType);
            var salt = hashProvider.GetSalt();
            var hash = hashProvider.GetHash(password, salt);

            User user = new User { UserId = users.Count, Username = username, Hash = hash, Salt = salt };
            users.Add(user);
        }

        public void UpdateUserById(int id, User newUser)
        {
            if (newUser == null) throw new ArgumentNullException();
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();
            if (id != newUser.UserId) throw new UserIdDoesNotMatchUserObjectIdException();

            User oldUser = this.GetActualUserById(id);
            users.Remove(oldUser);
            users.Add((User)newUser.Clone()); // we clone this to protect the reference
        }

        public void DeleteUserById(int id)
        {            
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();

            User user = this.GetActualUserById(id);
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
        public User GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return (User)users.First(x => x.UserId == id).Clone();
        }

        /// <summary>
        /// Return a clone of the user data object
        /// This is so that they are forced to use the update method
        /// rather than just changing the object directly.
        /// </summary>
        public User GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return (User)users.First(x => x.Username == username).Clone();
        }

        /// <summary>
        /// Return the actual object reference for the user
        /// We can use this to search by reference in the List<>
        /// </summary>
        protected User GetActualUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return users.First(x => x.UserId == id);
        }

        /// <summary>
        /// Return the actual object reference for the user
        /// We can use this to search by reference in the List<>
        /// </summary>
        protected User GetActualUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return users.First(x => x.Username == username);
        }

        public List<User> GetAllUsers()
        {
            List<User> copiedUsers = GenericCopier<List<User>>.DeepCopy(users);
            return copiedUsers;
        }

        /* this is only visible to EasyAuth.Tests */
        internal void Reset()
        {
            users = new List<User>();
        }
    }
}