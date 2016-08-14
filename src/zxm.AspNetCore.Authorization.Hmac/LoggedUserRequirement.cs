using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace zxm.AspNetCore.Authorization.Hmac
{
    public class LoggedUserRequirement : AuthorizationHandler<LoggedUserRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            LoggedUserRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.UserData))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
