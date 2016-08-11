using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace zxm.AspNetCore.Authentication.Bearer
{
    public class BearerMiddleware : AuthenticationMiddleware<BearerOptions>
    {
        public BearerMiddleware(RequestDelegate next, IOptions<BearerOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
            if (string.IsNullOrEmpty(Options.Token))
            {
                throw new ArgumentNullException(nameof(Options.Token));
            }
        }
        
        protected override AuthenticationHandler<BearerOptions> CreateHandler()
        {
            return new BearerHandler();
        }
    }
}
