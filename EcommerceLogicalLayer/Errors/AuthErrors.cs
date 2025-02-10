using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLogicalLayer.Errors
{
    public static class AuthErrors
    {


        public static string Invalid = "Invalid UserName Or Password";

        public static string InvalidRefreshToken = "Invalid Token Or RefreshToken";

        public static string Duplicated = "Duplicated Email";


        public static string ServerError = "Internal ServerError";

        public static string Confirmed = "Confirmed Your Email ";



    }
}
