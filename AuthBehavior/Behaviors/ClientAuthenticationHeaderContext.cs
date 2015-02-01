using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthBehavior
{
    public class ClientAuthenticationHeaderContext
    {
        public static AuthenticationData HeaderInformation;

        static ClientAuthenticationHeaderContext()
        {
            HeaderInformation = new AuthenticationData();
        }
    }
}