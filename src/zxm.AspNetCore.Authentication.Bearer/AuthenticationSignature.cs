using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxm.AspNetCore.Authentication.Bearer
{
    public class AuthenticationSignature
    {
        public string ClientId { get; set; }

        public string Signature { get; set; }

        public int Timestamp { get; set; }


    }
}
