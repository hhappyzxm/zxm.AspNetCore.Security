using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace zxm.AspNetCore.Identity
{
    public class IdentityUser : IIdentity
    {
        public IdentityUser(string name)
        {
            Name = name;
        }

        public string AuthenticationType { get; set; } = "HMAC";

        public bool IsAuthenticated => !Name.Equals("");

        public string Name { get; }

        public string UserAccessToken { get; set; }
    }
}
