using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace zxm.AspNetCore.Authentication.Bearer
{
    public class BearerOptions : AuthenticationOptions
    {
        public BearerOptions()
        {
            AuthenticationScheme = BearerDefaults.AuthenticationScheme;
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }

        public string Token { get; set; }
    }
}
