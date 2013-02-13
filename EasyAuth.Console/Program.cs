using EasyAuth.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuth.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityUserStore userStore = EntityUserStore.Instance;
            userStore.ConnectionString = "Data Source=(localdb)\v11.0;Initial Catalog=Test2;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFrameworkMUE";

            userStore.AddUser("TestUser", "TestPass");

            string cs = userStore.GetConnectionString();

            Debug.WriteLine("ConnectionString: {0}", cs);
            System.Console.WriteLine("ConnectionString: {0}", cs);
            System.Console.ReadKey();
        }
    }
}
