using System;

namespace EasyAuth
{
    public class Authentication<T> : IDisposable where T : IUserStore, new()
    {
        public bool IsAuthenticated { get; private set; }

        private IUserStore userStore;

        public Authentication()
        {
            userStore = new T();
        }        

        public void CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            userStore.Dispose();
        }
    }
}