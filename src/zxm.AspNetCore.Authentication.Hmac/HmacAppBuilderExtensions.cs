using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Internal;
using zxm.AspNetCore.RequestBodyRewind;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public static class HmacAppBuilderExtensions
    {
        public static IApplicationBuilder UseHmacAuthentication(this IApplicationBuilder app, HmacOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseRequestBodyRewind();

            return app.UseMiddleware<HmacMiddleware>(Options.Create(options));
        }
    }
}
