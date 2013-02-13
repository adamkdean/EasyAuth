using System;
using System.Linq;
using System.Collections.Generic;
using EasyAuth.Helpers;

namespace EasyAuth.Storage
{
    /// <summary>
    /// Stores users in a database using SQL.    
    /// </summary>
    public class EntityUserStore : IUserStore
    {
        static EntityUserStore instance = null;
        static readonly object padlock = new object();

        EntityUserStore() { }

        public static EntityUserStore Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new EntityUserStore();
                    return instance;
                }
            }
        }

        public static void Reset()
        {
            //
        }

        public string ConnectionString { get; set; }

        public string GetConnectionString()
        {
            using (var context = new UserStoreContext(ConnectionString))
            {
                return context.Database.Connection.ConnectionString;
            }
        }

        public void AddUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if(this.UserExistsByUsername(username)) throw new UserAlreadyExistsException();

            using (var context = new UserStoreContext(ConnectionString))
            {
                User userA = new User { Username = username, Password = password };
                context.Users.Add(userA);
                context.SaveChanges();
            }
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
            using (var context = new UserStoreContext(ConnectionString))
            {
                return context.Users.Any(x => x.UserId == id);
            }
        }

        public bool UserExistsByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            using (var context = new UserStoreContext(ConnectionString))
            {
                return context.Users.Any(x => x.Username == username);
            }
        }

        public User GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!UserExistsById(id)) throw new UserDoesNotExistException();

            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!UserExistsByUsername(username)) throw new UserDoesNotExistException();

            using (var context = new UserStoreContext(ConnectionString))
            {
                return (User)context.Users.First(x => x.Username == username);
            }
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();

            /*using (var context = new UserStoreContext())
            {
                return List<User>context.Users;
            }*/
        }
    }
}