using System;
using System.Collections.Generic;

namespace EasyAuth
{
    public interface IUserStore
    {
        void AddUser(string username, string password);
        
        void UpdateUserById(int id, UserData user);
        
        void DeleteUserById(int id);
        
        bool UserExistsById(int id);
        bool UserExistsByUsername(string username);

        UserData GetUserById(int id);
        UserData GetUserByUsername(string username);
        List<UserData> GetAllUsers();
    }
}