using System;
using System.Linq;
using System.Collections.Generic;

namespace EasyAuth.Storage
{
    /// <summary>
    /// Stores users in a database using RavenDB.    
    /// </summary>
    public class RavenUserStore : IUserStore
    {
        static RavenUserStore instance = null;
        static readonly object padlock = new object();

        RavenUserStore() { }

        public static RavenUserStore Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new RavenUserStore();
                    return instance;
                }
            }
        }

        public static void Reset()
        {
            //
        }

        public void AddUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if(this.UserExistsByUsername(username)) throw new UserAlreadyExistsException();

            throw new NotImplementedException();            
        }

        public void UpdateUserById(int id, User newUser)
        {
            if (newUser == null) throw new ArgumentNullException();
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();
            
            throw new NotImplementedException();
        }

        public void DeleteUserById(int id)
        {            
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();

            throw new NotImplementedException();
        }

        public bool UserExistsById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            
            throw new NotImplementedException();
        }

        public User GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");

            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}