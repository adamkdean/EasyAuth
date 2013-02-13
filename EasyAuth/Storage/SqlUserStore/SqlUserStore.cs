using System;
using System.Linq;
using System.Collections.Generic;

namespace EasyAuth.Storage
{
    /// <summary>
    /// Stores users in a database using SQL.    
    /// </summary>
    public class SqlUserStore : IUserStore
    {
        static SqlUserStore instance = null;
        static readonly object padlock = new object();

        SqlUserStore() { }

        public static SqlUserStore Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new SqlUserStore();
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

        public void UpdateUserById(int id, UserData newUserData)
        {
            if (newUserData == null) throw new ArgumentNullException();
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

        public UserData GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");

            throw new NotImplementedException();
        }

        public UserData GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            throw new NotImplementedException();
        }

        public List<UserData> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}