using EasyAuth.Security;
using System;
using System.Linq;
using System.Web;

namespace EasyAuth
{
    public class Authentication
    {
        private const string COOKIE_NAME = "EasyAuthCookie";
        private const string SESSION_NAME = "EasyAuthSession";

        public static bool LockCookieToIP { get; set; }
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME] != null)
                    return (User)HttpContext.Current.Session[SESSION_NAME];
                else return null;
            }
            private set
            {
                HttpContext.Current.Session[SESSION_NAME] = value;
            }
        }
        public static IUserStore UserStore { get; set; }
        public static HttpContext HttpContext { get; set; }

        private static Type hashProviderType = typeof(SHA256HashProvider);
        public static Type HashProviderType
        {
            get { return hashProviderType; }
            set { hashProviderType = value; }
        }

        public static bool Authenticate()
        {
            HttpCookie cookie = GetCookie();
            if (cookie != null)
            {
                string cookieName = cookie.Values["name"];
                string cookieHash = cookie.Values["hash"];

                if (UserStore.UserExistsByUsername(cookieName))
                {
                    User user = UserStore.GetUserByUsername(cookieName);

                    var userHash = user.Hash;
                    if (LockCookieToIP) userHash = GetCookieHash(userHash, user.Salt, true);

                    if (userHash == cookieHash)
                    {
                        CurrentUser = user;
                        return true;
                    }
                }

                CurrentUser = null;
                ExpireCookie();
            }
            
            return false;
        }

        public static bool IsAuthenticated()
        {
            if (CurrentUser != null) return true;
            else return false;
        }

        public static bool Login(string username, string password) //, bool persist = false)
        {
            if (UserStore.UserExistsByUsername(username))
            {
                User user = UserStore.GetUserByUsername(username);

                var userHash = user.Hash;
                var passwordHash = GetCookieHash(password, user.Salt, false);
                if (LockCookieToIP)
                {
                    userHash = GetCookieHash(userHash, user.Salt, true);
                    passwordHash = GetCookieHash(passwordHash, user.Salt, true);
                }

                if (userHash == passwordHash)
                {
                    CreateCookie(user.Username, userHash);
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

        public static string HashPassword(string password, string salt)
        {
            return GetCookieHash(password, salt, false);
        }

        protected static string GetCookieHash(string hash, string salt, bool secure = false)
        {
            var hashProvider = (HashProvider)Activator.CreateInstance(hashProviderType);
            if (secure) hash = string.Format("{0}:{1}", hash, HttpContext.Request.UserHostAddress);
            return hashProvider.GetHash(hash, salt);
        }

        protected static void CreateCookie(string username, string hash)
        {
            HttpCookie cookie = new HttpCookie(COOKIE_NAME);
            cookie.Values.Add("name", username);
            cookie.Values.Add("hash", hash);
            cookie.Expires = DateTime.Now.AddDays(28); //TODO: Persist feature
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
            HttpCookie cookie = new HttpCookie(COOKIE_NAME);
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}