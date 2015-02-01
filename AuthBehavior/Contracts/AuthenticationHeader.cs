using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AuthBehavior
{
    [DataContract(Namespace = "http://chsakell.com")]
    public class AuthenticationHeader
    {
        [DataMember]
        public string EncryptedSignature { get; set; }
    }

    public class AuthenticationData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Timespan { get; set; }
    }
}