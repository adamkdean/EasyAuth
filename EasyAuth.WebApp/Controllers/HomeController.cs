using EasyAuth.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAuth.WebApp.Controllers
{
    [RequireAuthorization]
    public class HomeController : Controller
    {        
        //
        // GET: /Home/

        [AllowUnauthorized]
        public ActionResult Index()
        {            
            return View();
        }

        //
        // GET: /Home/Login

        [RequireUnauthorized]
        public ActionResult Login()
        {            
            return View();
        }

        //
        // POST: /Home/Login

        [HttpPost]
        [RequireUnauthorized]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                bool login_ok = Authentication.Login(model.Username, model.Password);
                ViewBag.Message = "Logged in ok? " + login_ok.ToString();
            }

            return View(model);
        }

        //
        // GET: /Home/Logout

        public ActionResult Logout()
        {
            Authentication.Logout();
            return View();
        }

        //
        // GET: /Home/MembersOnly

        public ActionResult MembersOnly()
        {
            return View();
        }

        //
        // GET: /Home/ListUsers
        
        [AllowUnauthorized]
        public ActionResult ListUsers()
        {            
            return View();
        }

        //
        // GET: /Home/CreateUser

        [AllowUnauthorized]
        public ActionResult CreateUser()
        {
            string username = "", password = "";

            for (int i = 0; i < 100; i++)
            {
                username = string.Format("User{0:D}", i);
                password = string.Format("Pass{0:D}", i);

                if (!Authentication.UserStore.UserExistsByUsername(username))
                {
                    Authentication.UserStore.AddUser(username, password);
                    break;
                }
                else
                {
                    continue;
                }
            }             

            ViewBag.Message = string.Format("User created, {0} : {1}", username, password);
            return View();
        }
    }
}
