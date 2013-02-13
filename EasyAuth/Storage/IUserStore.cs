using System;
using System.Collections.Generic;

namespace EasyAuth
{
    public interface IUserStore
    {
        void AddUser(string username, string password);
        
        void UpdateUserById(int id, User user);
        
        void DeleteUserById(int id);
        
        bool UserExistsById(int id);
        bool UserExistsByUsername(string username);

        User GetUserById(int id);
        User GetUserByUsername(string username);
        List<User> GetAllUsers();
    }
}