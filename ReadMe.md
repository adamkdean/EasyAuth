# EasyAuth 

EasyAuth is a simple, secure, and easy to use lightweight alternative to ASP.NET Membership.

## Features

The EasyAuth library is simple to use, and comes with a some common data storage options built in, including code-first Entity, SQL and RavenDB.

It is lightweight and does not come with a bunch of bloat-ware functions you'll never use.

## Example

This is how simple it is to use EasyAuth in an ASP.NET MVC4 web application:

### global.asax

    namespace EasyAuthExample
    {
        public class MvcApplication : System.Web.HttpApplication
        {
            // store the instance of the selected UserStore here
            static IUserStore UserStore = EntityUserStore.Instance;
            
            protected void Application_Start()
            {
                Authentication.UserStore = UserStore;
            }

            protected void Application_BeginRequest(Object sender, EventArgs e)
            {
                // we have to give feed the httpcontext through to the 
                // auth class at the beginning of each page request
                Authentication.HttpContext = HttpContext.Current;
            }
        }
    }

### Web.config

Make sure you remember to put the ConnectionString in for your selected data storage.

    <connectionStrings>
        <add name="DefaultConnection" 
            providerName="System.Data.SqlClient" 
            connectionString="Data Source=(LocalDb)\v11.0;Initial Catalog=EasyAuthExample;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\EasyAuthExample.mdf" />
    </connectionStrings>

Of course you can use other connection strings, as long as they're valid.

### HomeController.cs

Now you create a controller as you would with any regular MVC application, but instead of using the `[Authorize]` and `[AllowAnonymous]` attributes, you simply use `[EzAuthorize]` and `[EzAllowAnonymous]` instead.

Authenticate a user with `Authentication.Login(username, password)`, and log them out with `Authentication.Logout();`.

    namespace EasyAuthExample.Controllers
    {
        [EzAuthorize]
        public class HomeController : Controller
        {
            //
            // GET: /Home/
            
            [EzAllowAnonymous]
            public ActionResult Index()
            {            
                return View();
            }

            //
            // GET: /Home/Login
            
            [EzAllowAnonymous]
            public ActionResult Login()
            {            
                return View();
            }
            
            //
            // POST: /Home/Login
            
            [HttpPost]
            [EzAllowAnonymous]
            public ActionResult Login(LoginModel model)
            {
                if (ModelState.IsValid && Authentication.Login(model.Username, model.Password))
                {
                    return RedirectToAction("MembersOnly", "Home");
                }
            
                ViewBag.Message = "Invalid user credentials";
                return View(model);
            }
            
            //
            // GET: /Home/Logout
            
            public ActionResult Logout()
            {
                Authentication.Logout();
                return RedirectToAction("Index", "Home");
            }

            //
            // GET: /Home/MembersOnly
            
            public ActionResult MembersOnly()
            {
                return View();
            }
    }
}

And that's it!

## Create, Read, Update, Delete

It's easy to create, read, update and delete users.

### Create Users

Adding users is really easy, and for now just takes two arguments.

`void AddUser(string username, string password);`

    // first we check that the username doesn't already exist
    if (!Authentication.UserStore.UserExistsByUsername(username))
    {
        // then we simply add them to the database
        Authentication.UserStore.AddUser(username, password);
    }

### Read Users

The following methods are available to you via `Authentication.UserStore`.

    User GetUserById(int id);
    User GetUserByUsername(string username);
    List<User> GetAllUsers();

Example:

    User test = Authentication.UserStore.GetUserByUsername("TestUser");
    int testId = test.UserId;

    User test = Authentication.UserStore.GetUserById(14);
    string testUsername = test.Username;

    List<User> users = Authentication.UserStore.GetAllUsers();
    foreach(var user in users)
    {
        DoSomething.With(user.Username);
    }

### Update Users 

Updating users is really easy too. Make sure you pass the correct userId and User object!

`void UpdateUserById(int id, User user);`

    User test = Authentication.UserStore.GetUserByUsername("TestUser");
    test.Username = "IFancyANewName";
    Authentication.UserStore.UpdateUserById(test.UserId, test);

### Delete Users

When you have to delete users, sad as it is, you can do that too.

`void DeleteUserById(int id);`

    User test = Authentication.UserStore.GetUserByUsername("TestUser");
    Authentication.UserStore.DeleteUserById(test.UserId);

## License

Copyright (c) 2013, Adam K Dean, Lewis Ardern. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.