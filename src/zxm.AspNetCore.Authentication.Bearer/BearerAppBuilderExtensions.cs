using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace zxm.AspNetCore.Authentication.Bearer
{
    public static class BearerAppBuilderExtensions
    {
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder app, string token)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            return app.UseMiddleware<BearerMiddleware>(Options.Create(new BearerOptions {Token = token}));
        }
    }
}
