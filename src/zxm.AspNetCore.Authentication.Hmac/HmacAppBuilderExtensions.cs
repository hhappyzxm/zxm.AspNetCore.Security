using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public static class HmacAppBuilderExtensions
    {
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder app, HmacOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<HmacMiddleware>(Options.Create(options));
        }
    }
}
