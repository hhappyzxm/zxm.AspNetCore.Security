using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public enum VerifyUserAccessTokenState
    {
        Successed,
        Failed,
        Expired
    }
}
