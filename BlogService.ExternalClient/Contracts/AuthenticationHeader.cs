using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BlogService.ExternalClient.Contracts
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
