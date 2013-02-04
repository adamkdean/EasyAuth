using System;
using System.Collections.Generic;

namespace EasyAuth
{
    public class SimpleUserStore : IUserStore
    {
        private List<UserData> users;

        public SimpleUserStore()
        {
            users = new List<UserData>();
        }

        public void AddUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserById(int id, UserData user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public UserData GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public UserData GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            users = null;
        }
    }
}