using System;
using System.Linq;
using System.Web;

namespace EasyAuth
{
    public class Authentication
    {
        private const string COOKIE_NAME = "EasyAuthCookie";

        public static IUserStore UserStore { get; set; }
        public static HttpContext HttpContext { get; set; }
        public static UserData CurrentUser { get; private set; }

        public static bool Authenticate()
        {
            HttpCookie cookie = GetCookie();
            if (cookie != null)
            {                
                string name = cookie.Values["name"];
                string hash = cookie.Values["hash"];

                if (UserStore.UserExistsByUsername(name))
                {
                    UserData user = UserStore.GetUserByUsername(name);

                    // TODO: proper hash
                    if (user.Password == hash)
                    {
                        CreateCookie(user.Username, hash);
                        CurrentUser = user;
                        return true;
                    }                    
                }

                CurrentUser = null;
                ExpireCookie();
            }
            
            return false;
        }

        protected static void CreateCookie(string username, string hash)
        {
            // if cookie already exists, replace it
            //if (GetCookie() != null) ExpireCookie();

            HttpCookie cookie = new HttpCookie(COOKIE_NAME);
            cookie.Values.Add("name", username);
            cookie.Values.Add("hash", hash);
            HttpContext.Response.Cookies.Add(cookie);
        }

        protected static HttpCookie GetCookie()
        {
            var cookies = HttpContext.Current.Request.Cookies;
            if (cookies.AllKeys.Any(x => x == COOKIE_NAME)) 
                return cookies[COOKIE_NAME];
            else return null;
        }

        protected static void ExpireCookie()
        {
            HttpCookie cookie = GetCookie();
            if (cookie != null)
            {
                cookie.Expires.AddDays(-1);
            }
        }

        public static bool IsAuthenticated()
        {
            if (CurrentUser != null) return true;
            else return false;
        }

        public static bool Login(string username, string password)
        {
            if (UserStore.UserExistsByUsername(username))
            {
                UserData user = UserStore.GetUserByUsername(username);

                // TODO: proper hash
                if (user.Password == password)
                {
                    CurrentUser = user;
                    return true;
                }
            }

            CurrentUser = null;
            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
            ExpireCookie();
        }        
    }
}