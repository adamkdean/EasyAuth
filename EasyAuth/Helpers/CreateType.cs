using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuth.Helpers
{
    // Copyright © Judah Himango
    // http://stackoverflow.com/users/536/judah-himango

    public static class GenericType
    {
        public static T CreateType<T>() where T : new()
        {
            return new T();
        }
    }
}