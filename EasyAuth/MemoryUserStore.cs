using System;
using System.Linq;
using System.Collections.Generic;

namespace EasyAuth
{
    public class MemoryUserStore : IUserStore
    {
        private List<UserData> users;

        public MemoryUserStore()
        {
            users = new List<UserData>();
        }

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
            if (id != newUserData.UserId) throw new UserIdDoesNotMatchUserObjectId();

            UserData oldUserData = this.GetActualUserById(id);
            users.Remove(oldUserData);
            users.Add(newUserData);
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

        public UserData GetUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return (UserData)users.First(x => x.UserId == id).Clone();
        }

        public UserData GetUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return (UserData)users.First(x => x.Username == username).Clone();
        }

        protected UserData GetActualUserById(int id)
        {
            if (id < 0) throw new ArgumentException("id");
            if (!users.Any(x => x.UserId == id)) throw new UserDoesNotExistException();
            return users.First(x => x.UserId == id);
        }

        protected UserData GetActualUserByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!users.Any(x => x.Username == username)) throw new UserDoesNotExistException();
            return users.First(x => x.Username == username);
        }

        public void Dispose()
        {
            users = null;
        }
    }
}