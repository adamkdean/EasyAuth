using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace EasyAuth.Storage
{
    public class UserStoreContext : DbContext
    {
        public UserStoreContext()
            : base("DefaultConnection")
        {
        }

        public UserStoreContext(string connectionString = "DefaultConnection")
            : base(connectionString)
        {
        }
    
        public DbSet<User> Users { get; set; }
    }
}