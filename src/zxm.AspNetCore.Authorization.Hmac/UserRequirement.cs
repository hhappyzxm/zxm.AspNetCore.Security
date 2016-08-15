using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using zxm.AspNetCore.Authentication.Hmac.Identity;

namespace zxm.AspNetCore.Authorization.Hmac
{
    public class UserRequirement : AuthorizationHandler<UserRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            UserRequirement requirement)
        {
            if (context.User != null && context.User.Identity is HmacIdentity)
            {
                if (!string.IsNullOrEmpty(((HmacIdentity) context.User.Identity).UserAccessToken))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.FromResult(0);
        }
    }
}
