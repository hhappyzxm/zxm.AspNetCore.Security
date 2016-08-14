using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public class HmacMiddleware : AuthenticationMiddleware<HmacOptions>
    {
        public HmacMiddleware(RequestDelegate next, IOptions<HmacOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
            if (Options.TryGetClientSecret == null)
            {
                throw new ArgumentNullException(nameof(Options.TryGetClientSecret));
            }
        }
        
        protected override AuthenticationHandler<HmacOptions> CreateHandler()
        {
            return new HmacHandler();
        }
    }
}
