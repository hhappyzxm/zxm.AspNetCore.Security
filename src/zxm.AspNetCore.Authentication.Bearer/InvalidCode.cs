using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxm.AspNetCore.Authentication.Bearer
{
    public enum InvalidCode
    {
        None = -1,

        InvalidHttpMethod = 1000,

        EmptyQueryString = 2000,

        EmptyClientId = 2000,
        InvalidClientId = 2001,

        InvalidSignature = 3000
    }
}
