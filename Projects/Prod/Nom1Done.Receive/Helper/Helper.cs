using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.Receive.Helper
{
    public static class Helper
    {
        public static bool VaidateUser(string username, string password)
        {
            if (username.ToLower() == "appenerprod" && password == "EnerProd99")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}