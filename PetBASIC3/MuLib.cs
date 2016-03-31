using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBASIC3
{
    public static class MuLib
    {
        public static T As<T>(this object o)
        {
            return (T) o;
        }
    }
}
