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

        public static bool IsAuthenticated()
        {
            if (CurrentUser != null) return true;

            var cookie = GetCookie();
            if (cookie != null)
            {
                var cookieName = cookie.Values["name"];
                var cookieHash = cookie.Values["hash"];

                return Authenticate(cookieName, cookieHash);
            }

            return false;
        }

        public static bool Login(string username, string password)
        {
            return Login(username, password, persist: false);
        }

        public static bool Login(string username, string password, bool persist, int length = 28)
        {
            if (UserStore.UserExistsByUsername(username))
            {
                var user = UserStore.GetUserByUsername(username);
                var suppliedHash = HashPassword(password, user.Salt);

                return Authenticate(username, suppliedHash, persist, length);
            }
            
            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
            ExpireCookie();
        }

        public static string HashPassword(string password, string salt, bool secure = false)
        {
            var hashProvider = (HashProvider)Activator.CreateInstance(hashProviderType);
            if (secure) password = string.Format("{0}:{1}", password, HttpContext.Request.UserHostAddress);
            return hashProvider.GetHash(password, salt);
        }

        protected static bool Authenticate(string username, string hash)
        {
            return Authenticate(username, hash, persist: false);
        }

        protected static bool Authenticate(string username, string hash, bool persist, int length = 28)
        {
            if (UserStore.UserExistsByUsername(username))
            {
                var user = UserStore.GetUserByUsername(username);
                var existingHash = user.Hash;
                var suppliedHash = hash;
                
                // if we want sessions/cookies locked to IP then we need to hash the hashes again with the IP
                // example: ThisIsAHash -> 127.0.0.1:ThisIsAHash -> NewUniqueHash etc
                if (LockCookieToIP)
                {
                    existingHash = HashPassword(existingHash, user.Salt, secure: true);
                    suppliedHash = HashPassword(suppliedHash, user.Salt, secure: true);
                }

                if (existingHash == suppliedHash)
                {
                    if (persist) CreateCookie(user.Username, existingHash);
                    CurrentUser = user;
                    return true;
                }
            }

            CurrentUser = null;
            return false;
        }
        
        protected static void CreateCookie(string username, string hash, int days = 28)
        {
            var cookie = new HttpCookie(COOKIE_NAME);
            cookie.Values.Add("name", username);
            cookie.Values.Add("hash", hash);
            cookie.Expires = DateTime.Now.AddDays(days);
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
            var cookie = new HttpCookie(COOKIE_NAME);
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}