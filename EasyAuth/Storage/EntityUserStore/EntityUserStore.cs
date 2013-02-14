using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyAuth.Helpers;
using EntityFramework.Extensions;

/* We only want the Tests to be able to call IUserStore.Reset() */
[assembly: InternalsVisibleTo("EasyAuth.Tests")]

namespace EasyAuth.Storage
{
    /// <summary>
    /// Stores users in a database using SQL.    
    /// </summary>
    public class EntityUserStore : IUserStore
    {
        #region Singleton
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
        #endregion

        private Type contextType = typeof(UserStoreContext);

        public void SetContext(Type type)
        {
            contextType = type;
        }

        public void AddUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if(this.UserExistsByUsername(username)) throw new UserAlreadyExistsException();

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                User user = new User { Username = username, Password = password };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void UpdateUserById(int id, User newUser)
        {
            if (newUser == null) throw new ArgumentNullException();
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();
            if (id != newUser.UserId) throw new UserIdDoesNotMatchUserObjectIdException();

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                User user = (User)context.Users.First(x => x.UserId == id);
                user.Username = newUser.Username;
                user.Password = newUser.Password;
                context.SaveChanges();
            }
        }

        public void DeleteUserById(int id)
        {            
            if (!this.UserExistsById(id)) throw new UserDoesNotExistException();

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                context.Users.Delete(x => x.UserId == id);
            }
        }

        public bool UserExistsById(int id)
        {
            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                return context.Users.Any(x => x.UserId == id);
            }
        }

        public bool UserExistsByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                return context.Users.Any(x => x.Username == username);
            }
        }

        public User GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!UserExistsById(id)) throw new UserDoesNotExistException();

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                return (User)context.Users.First(x => x.UserId == id);
            }
        }

        public User GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!UserExistsByUsername(username)) throw new UserDoesNotExistException();

            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                return (User)context.Users.First(x => x.Username == username);
            }
        }

        public List<User> GetAllUsers()
        {
            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                return context.Users.ToList<User>();
            }
        }

        internal void Reset()
        {
            using (var context = (UserStoreContext)Activator.CreateInstance(contextType))
            {
                context.Users.Delete();
                context.SaveChanges();
            }
        }
    }
}